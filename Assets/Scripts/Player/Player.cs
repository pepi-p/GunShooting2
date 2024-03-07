using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;
using UnityEngine;
using UnityEngine.UI;
using Cursor = UnityEngine.Cursor;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [Header("Class")]
    [SerializeField] private Arduino arduino;
    [SerializeField] private SerialHandler serialHandler;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private Result result;

    [Space(5), Header("Prefab")]
    [SerializeField] private PlayerBullet bullet;   // 弾のプレファブ
    [SerializeField] private GameObject particle;   // パーティクルのプレファブ

    [Space(5), Header("Setting")]
    [SerializeField] private float shotInterval;    // 射撃の間隔
    [SerializeField] private float shotDamage;      // 与ダメージ
    [SerializeField] private float hp;              // hp
    [SerializeField] private bool infinityAmmo;     // これが有効なときは弾が無限になる
    public bool allowShot = true;                   // プレイヤーが撃てないようにする（true: 撃てる）

    [Space(5), Header("Assign")]
    [SerializeField] private Transform camParent;   // カメラの親
    [SerializeField] private RectTransform reticle; // 照準の座標
    [SerializeField] private RectTransform uiRoot;  // UI全体
    [SerializeField] private Image playerHPbar;     // プレイヤーのHPバー
    [SerializeField] private Image empty;           // 残弾が空のときに表示させる画像
    [SerializeField] private Image reloading;       // リロード中に表示させる画像
    [SerializeField] private Image damageImage;     // 被弾時に表示させる画像
    [SerializeField] private Light shotFlash;       // マズルフラッシュのライト
    [SerializeField] private AudioClip shotSE;      // 射撃時の効果音

    private AudioSource audioSource;                // オーディオソース
    private Camera cam;                             // カメラ
    private Color damageColor;                      // ダメージを受けたときの画像の色
    private float shotCoolDown;                     // 次弾発射までの時間
    private float reticleValue;                     // 振動の度合
    private int ammo = 100;                         // 残弾
    private float maxHP;                            // 最大体力
    private float shotFlashDelay;                   // マズルフラッシュ
    private float hideWeight = 0;                   // 隠れているか
    private float emptyAlpha = 0;                   // 弾倉が空になった時の表示の透明度
    private bool isReload;                          // リロード中か
    private float damageAlpha = 0;                  // ダメージを受けた時の画像の透明度
    private Vector3 pointerPos;                     // ポインターの座標
    private bool magazineReload;                    // マガジンを外しているか（リロード時）
    private bool easyReload;                        // 隠れるだけでリロードするのを許可

    private void Start()
    {
        easyReload = TitleManager.easyReload;
        audioSource = GetComponent<AudioSource>();
        cam = Camera.main;
        Cursor.visible = false;
        maxHP = hp;
        damageColor = damageImage.color;

        StartCoroutine(UpdateAmmo());
    }

    private void Update()
    {
        var shotInput = Input.GetMouseButton(0);

        // Arduinoが接続されているとき
        if (Setting.isArduino)
        {
            var yaw = arduino.yaw;
            if (yaw > 180) yaw -= 360;
            
            // ピッチとヨーからポインターの座標を計算
            pointerPos = new Vector3(Mathf.Atan(yaw * Mathf.Deg2Rad) * Setting.pointerOffset, Mathf.Atan(arduino.pitch * Mathf.Deg2Rad) * Setting.pointerOffset, 0) + new Vector3(Screen.width, Screen.height) / 2;
            
            // 撃つ入力を銃のトリガーにする
            shotInput = arduino.trigger;
            
            // easyReloadが有効なときはhideで，無効なときはマガジンを外すことでリロード
            if ((!easyReload && arduino.magazine) || (easyReload && arduino.hide))
            {
                magazineReload = true;
                StartCoroutine(MortorSetEnable(false));
                reloading.GetComponent<Image>().enabled = true;
            }
            // マガジンが再度装着されたらリロード終了
            else if (magazineReload)
            {
                magazineReload = false;
                ammo = 100;
                StartCoroutine(MortorSetEnable(true));
                serialHandler.Write("100\n");
                uiManager.SetAmmoValue(ammo);
                reloading.GetComponent<Image>().enabled = false;
            }
        }
        // 銃が接続されていない場合はポインターの座標をマウスの座標にする
        else pointerPos = Input.mousePosition;
        
        // 隠れてる間は撃てないようにする
        if (hideWeight > 0.1f)
        {
            shotInput = false;
        }

        // 装填(デバッグ用)
        if (!isReload && Input.GetKeyDown(KeyCode.R)) StartCoroutine(DebugReload());

        // 射撃
        if (allowShot && shotInput && shotCoolDown <= 0 && !isReload)
        {
            Shot();
            shotCoolDown = shotInterval;
            reticleValue = 1;
            shotFlashDelay = 1;
        }
        if (shotCoolDown > 0) shotCoolDown -= Time.deltaTime;

        // 弾倉空の時の表示
        emptyAlpha = Mathf.Lerp(emptyAlpha, 0, Time.deltaTime * 16);
        var emptyColor = empty.color;
        empty.color = new Color(emptyColor.r, emptyColor.g, emptyColor.b, emptyAlpha);

        // ポインタ移動
        Cursor.lockState = CursorLockMode.None;
        reticle.position = pointerPos;

        // 射撃時にUI全体を振動させる処理
        reticleValue = Mathf.Lerp(reticleValue, 0, Time.deltaTime * 4);
        uiRoot.localScale = Vector2.one + Vector2.one * (reticleValue * 0.015f);
        
        // マズルフラッシュの光源処理
        if (shotFlashDelay > 0.5f) shotFlashDelay = Mathf.Lerp(shotFlashDelay, 0, Time.deltaTime * 25);
        else shotFlashDelay = Mathf.Lerp(shotFlashDelay, 0, Time.deltaTime * 5);
        shotFlash.range = shotFlashDelay * 15;

        // 被弾時の画像表示
        damageColor.a = damageAlpha;
        damageImage.color = damageColor;
        if (damageAlpha > 0) damageAlpha -= Time.deltaTime * 0.2f;
        
        // 隠れる処理
        if (Input.GetKey(KeyCode.Space) || arduino.hide || magazineReload) hideWeight = Mathf.Lerp(hideWeight, 1, Time.deltaTime * 10);
        else hideWeight = Mathf.Lerp(hideWeight, 0, Time.deltaTime * 10);
        cam.transform.position = Vector3.Lerp(camParent.position, camParent.position + Vector3.down * 0.5f, hideWeight);
    }

    /// <summary>
    /// マウス操作の時のリロード処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator DebugReload()
    {
        isReload = true;
        reloading.enabled = true;
        reticle.GetComponent<Image>().enabled = false;
        yield return new WaitForSecondsRealtime(2);
        isReload = false;
        reloading.enabled = false;
        reticle.GetComponent<Image>().enabled = true;
        ammo = 100;
        uiManager.SetAmmoValue(ammo);
    }

    /// <summary>
    /// 射撃処理
    /// </summary>
    private void Shot()
    {
        // 残弾がなければEMPTYの画像を表示する
        if (ammo == 0)
        {
            emptyAlpha = 1;
            return;
        }
        
        audioSource.PlayOneShot(shotSE);
        ammo--;
        scoreManager.shotCount++;
        
        // 残弾表示の更新
        uiManager.SetAmmoValue(ammo);
        
        // 銃側のディスプレイの更新（うまく動かなかった）
        // serialHandler.Write(ammo.ToString() + "\n");
        
        // infinityAmmoが有効な時，弾を増やす（デバッグ用）
        if (infinityAmmo && ammo == 0) ammo = 9;
        
        // 残弾がなくなったらモーターを停止
        if (ammo <= 0 && Setting.isArduino) StartCoroutine(MortorSetEnable(false));
        
        // Raycastを飛ばす
        var isHit = Physics.Raycast(cam.ScreenPointToRay(pointerPos), out var hit, 50f, 1);
        
        // 弾と着弾地点に火花のパーティクルを表示する
        Instantiate(particle, hit.point, Quaternion.LookRotation(cam.transform.position - hit.point));
        Instantiate(bullet, (this.transform.position + Vector3.up * 0.9f), Quaternion.LookRotation(hit.point - (this.transform.position + Vector3.up * 0.9f)));
        
        if (isHit && hit.collider != null)
        {
            // ダメージ処理
            if (hit.collider.TryGetComponent<IDamage>(out var target))
            {
                target.AddDamage(shotDamage);
                scoreManager.shotHitCount++;
            }
        }
    }

    /// <summary>
    /// 被弾時の処理
    /// </summary>
    /// <param name="damage">受けるダメージ</param>
    public void Hit(float damage)
    {
        // 隠れている間はダメージを受けない
        if (hideWeight > 0.4f) return;
        
        scoreManager.AddScore(-10 * (int)damage);
        hp -= damage;
        playerHPbar.fillAmount = hp / maxHP;
        damageAlpha = 110 / 255f;
        
        // 体力がなくなったらゲームオーバーを表示する
        if (hp <= 0) result.DisplayGameOver();
    }

    /// <summary>
    /// HP割合を取得する
    /// </summary>
    /// <returns>HP割合</returns>
    public float GetHPRate()
    {
        return hp / maxHP;
    }

    /// <summary>
    /// モーターの有効/無効の切り替え
    /// </summary>
    /// <param name="enable">モーターの有効(true)/無効(false)</param>
    /// <returns></returns>
    private IEnumerator MortorSetEnable(bool enable)
    {
        while (true)
        {
            // arudino側から送信される内容とenbaleが一致するまでモーターの制御情報を送信する
            if (arduino.mortorEnable != enable) arduino.MotorStop(enable);
            else break;

            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// 銃側のディスプレイとUnity側で残弾に誤差があった場合に訂正する
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateAmmo()
    {
        while (true)
        {
            if (arduino.displayAmmo != ammo) serialHandler.Write(ammo.ToString() + "\n");

            yield return new WaitForSeconds(0.2f);
        }
    }
}
