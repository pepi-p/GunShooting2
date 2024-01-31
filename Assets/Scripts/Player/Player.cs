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
    [SerializeField] private PlayerBullet bullet;
    [SerializeField] private GameObject particle;

    [Space(5), Header("Setting")]
    [SerializeField] private float shotInterval;
    [SerializeField] private float shotDamage;
    [SerializeField] private float hp;
    [SerializeField] private bool infinityAmmo;
    public bool allowShot = true;

    [Space(5), Header("Assign")]
    [SerializeField] private Transform camParent;
    [SerializeField] private RectTransform reticle;
    [SerializeField] private RectTransform reticleImage;
    [SerializeField] private RectTransform[] reticleSub;
    [SerializeField] private RectTransform ui;
    [SerializeField] private Image playerHPbar;
    [SerializeField] private Image enemyHPbar;
    [SerializeField] private Image empty;
    [SerializeField] private Image reloading;
    [SerializeField] private Image damageImage;
    [SerializeField] private Light shotFlash;
    [SerializeField] private AudioClip shotSE;

    private AudioSource audioSource;
    private Camera cam;
    private Color damageColor;
    private float shotCoolDown;
    private float reticleValue;
    private int ammo = 100;
    private float maxHP;
    private float shotFlashDelay;
    private float hideWeight = 0;
    private float emptyAlpha = 0;
    private bool isReload;
    private float damageAlpha = 0;
    private Vector3 pointerPos;
    private bool magazineReload;
    private bool easyReload;

    private void Start()
    {
        easyReload = TitleManager.easyReload;
        if (easyReload) hp *= 2;
        audioSource = GetComponent<AudioSource>();
        cam = Camera.main;
        Cursor.visible = false;
        maxHP = hp;
        damageColor = damageImage.color;
    }

    private void Update()
    {
        var mouseMainInput = Input.GetMouseButton(0);

        if (Setting.isArduino)
        {
            var yaw = arduino.yaw;
            if (yaw > 180) yaw -= 360;
            pointerPos = new Vector3(Mathf.Atan(yaw * Mathf.Deg2Rad) * Setting.pointerOffset, Mathf.Atan(arduino.pitch * Mathf.Deg2Rad) * Setting.pointerOffset, 0) + new Vector3(Screen.width, Screen.height) / 2;
            mouseMainInput = arduino.trigger;
            if ((!easyReload && arduino.magazine) || (easyReload && arduino.hide))
            {
                magazineReload = true;
                arduino.MotorStop(false);
                reloading.GetComponent<Image>().enabled = true;
            }
            else if (magazineReload)
            {
                magazineReload = false;
                ammo = 100;
                arduino.MotorStop(true);
                uiManager.SetAmmoValue(ammo);
                reloading.GetComponent<Image>().enabled = false;
            }
        }
        else pointerPos = Input.mousePosition;
        
        if (hideWeight > 0.1f)
        {
            mouseMainInput = false;
        }

        // 装填(デバッグ用)
        if (!isReload && Input.GetKeyDown(KeyCode.R)) StartCoroutine(DebugReload());

        // 射撃
        if (allowShot && mouseMainInput && shotCoolDown <= 0 && !isReload) Shot();
        if (shotCoolDown > 0) shotCoolDown -= Time.deltaTime;

        // 弾倉空の時の表示
        emptyAlpha = Mathf.Lerp(emptyAlpha, 0, Time.deltaTime * 16);
        var emptyColor = empty.color;
        empty.color = new Color(emptyColor.r, emptyColor.g, emptyColor.b, emptyAlpha);

        // ポインタ移動
        Cursor.lockState = CursorLockMode.None;
        reticleImage.position = pointerPos;

        // 射撃時にUI全体を振動させる処理
        reticleValue = Mathf.Lerp(reticleValue, 0, Time.deltaTime * 4);
        ui.localScale = Vector2.one + Vector2.one * (reticleValue * 0.015f);
        
        // マズルフラッシュの光源処理
        if (shotFlashDelay > 0.5f) shotFlashDelay = Mathf.Lerp(shotFlashDelay, 0, Time.deltaTime * 25);
        else shotFlashDelay = Mathf.Lerp(shotFlashDelay, 0, Time.deltaTime * 5);
        shotFlash.range = shotFlashDelay * 15;

        damageColor.a = damageAlpha;
        damageImage.color = damageColor;
        if (damageAlpha > 0) damageAlpha -= Time.deltaTime * 0.2f;
    }

    private void FixedUpdate()
    {
        Hide();
    }

    private void Hide()
    {
        if (Input.GetKey(KeyCode.Space) || arduino.hide || magazineReload) hideWeight = Mathf.Lerp(hideWeight, 1, Time.deltaTime * 10);
        else hideWeight = Mathf.Lerp(hideWeight, 0, Time.deltaTime * 10);

        this.tag = hideWeight > 0.4f ? "Untagged" : "Player";

        cam.transform.position = Vector3.Lerp(camParent.position, camParent.position + Vector3.down * 0.5f, hideWeight);
    }

    private IEnumerator DebugReload()
    {
        isReload = true;
        reloading.enabled = true;
        reticleImage.GetComponent<Image>().enabled = false;
        yield return new WaitForSecondsRealtime(2);
        isReload = false;
        reloading.enabled = false;
        reticleImage.GetComponent<Image>().enabled = true;
        ammo = 100;
        uiManager.SetAmmoValue(ammo);
    }

    private void Shot()
    {
        if (ammo == 0)
        {
            emptyAlpha = 1;
            return;
        }
        shotCoolDown = shotInterval;
        reticleValue = 1;
        shotFlashDelay = 1;
        audioSource.PlayOneShot(shotSE);
        ammo--;
        if (ammo <= 0) arduino.MotorStop(true);
        scoreManager.shotCount++;
        if (infinityAmmo && ammo == 0) ammo = 9;
        uiManager.SetAmmoValue(ammo);
        serialHandler.Write(ammo.ToString() + "\n");
        var isHit = Physics.Raycast(cam.ScreenPointToRay(pointerPos), out var hit, 50f, 1);
        Instantiate(particle, hit.point, Quaternion.LookRotation(cam.transform.position - hit.point));
        Instantiate(bullet, (this.transform.position + Vector3.up * 0.9f), Quaternion.LookRotation(hit.point - (this.transform.position + Vector3.up * 0.9f)));
        if (isHit && hit.collider != null) {
            if (hit.collider.CompareTag("Enemy"))
            {
                scoreManager.shotHitCount++;
                var enemy = hit.transform.root.GetComponentInParent<Enemy>();
                if (Input.GetKey(KeyCode.LeftShift)) enemy.Hit(shotDamage * 100);
                else
                {
                    if (hit.collider.name == "Eye") enemy.Hit(30 * shotDamage);
                    else enemy.Hit(shotDamage);
                }
            }
            else if (hit.collider.CompareTag("Boss"))
            {
                scoreManager.shotHitCount++;
                var target = hit.collider.GetComponent<BossTarget>();
                target.Damage(shotDamage);
            }
            else if (hit.collider.CompareTag("Missile"))
            {
                scoreManager.AddScore(300);
                scoreManager.shotHitCount++;
                var missile = hit.transform.root.GetComponentInParent<Missile>();
                missile.StartCoroutine(missile.Hit());
            }
        }
    }

    public void Hit(float damage)
    {
        scoreManager.AddScore(-10 * (int)damage);
        hp -= damage;
        playerHPbar.fillAmount = hp / maxHP;
        damageAlpha = 110 / 255f;
        if (hp <= 0)
        {
            result.DisplayGameOver();
        }
    }

    public float GetHPRate()
    {
        return hp / maxHP;
    }
}
