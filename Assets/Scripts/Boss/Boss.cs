using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
{
    [Header("Class")]
    [SerializeField] private Player player;
    [SerializeField] private Result result;

    [SerializeField] private TimelineManager timelineManager;

    [Space(5), Header("Prefab")]
    [SerializeField] private EnemyBullet bullet;

    [Space(5), Header("Setting")]
    [SerializeField] private float shotInterval;
    [SerializeField] private float shotIntervalGun;
    [SerializeField] private float gatlingRotateSpeed;
    [SerializeField] private float bossGunRandomRange;
    [SerializeField] private float bossGunShotDamage;

    [Space(5), Header("Assign")]
    [SerializeField] private Transform[] armL; // 0 : Zaxis, 1 : Xaxis, 2 : Yaxis
    [SerializeField] private Transform[] armR; // 0 : Zaxis, 1 : Xaxis, 2 : Yaxis
    [SerializeField] private Transform muzzleL;
    [SerializeField] private Transform muzzleR;
    [SerializeField] private GameObject armL2;
    [SerializeField] private GameObject armR2;
    [SerializeField] private Image bossHPbar;
    [SerializeField] private Image bossHPbarBackGround;
    [SerializeField] private BossTarget[] bossTargets;
    [SerializeField] private BossGun[] bossGuns;
    [SerializeField] private SEPlayer sePlayer; 
    
    private Animator animator;

    private float hp;
    private float maxHP = 0;
    private int phase = 0;
    private float hpBarAlpha;
    private float gatlingRotate = 0;

    private Quaternion beamCannonQuaternion;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        result.isBossBattle = true;

        // HPを初期化
        // ターゲットのHPの合計値を最大HPにする
        foreach (var target in bossTargets) maxHP += target.hp;
        hp = maxHP;
        
        bossHPbarBackGround.gameObject.SetActive(true);
    }

    private void Update()
    {
        // HPを計算
        hp = 0;
        var phaseHP = 0f;
        foreach (var target in bossTargets)
        {
            hp += target.hp;
            if (target.phase == phase) phaseHP += target.hp;
        }
        if(phaseHP <= 0) NextPhase();
        bossHPbar.fillAmount = hp / maxHP;
        bossHPbarBackGround.color = Color.Lerp(new Color(58f / 255f, 58f /255f, 58f /255f), new Color(207f / 255f, 207f / 255f, 207f / 255f), hpBarAlpha * hpBarAlpha);
        if (hpBarAlpha > 0) hpBarAlpha -= Time.deltaTime * 2; 
    }

    private void NextPhase()
    {
        phase++;
        foreach (var target in bossTargets)
        {
            target.SetEnable(target.phase == phase);
        }
    }

    public void Damage()
    {
        hpBarAlpha = 1;
    }

    public void AttackAnimation(float length)
    {
        StartCoroutine(TurnGattling(length));
    }

    private IEnumerator TurnGattling(float length)
    {
        float time = 0;
        float shotCoolDown = 0;
        while (time <= length)
        {
            time += Time.deltaTime;
            gatlingRotate += Time.deltaTime * gatlingRotateSpeed;
            shotCoolDown += Time.deltaTime;
            if (gatlingRotate > 180f) gatlingRotate = -180f;
            armL[2].localRotation = Quaternion.Euler(0, -gatlingRotate, 0);
            armR[2].localRotation = Quaternion.Euler(0, gatlingRotate, 0);
            if (shotCoolDown >= shotInterval)
            {
                shotCoolDown = 0;
                Shot();
                // if (time / length > 0.4f && player.CompareTag("Player")) player.Hit(0);
            }
            yield return null;
        }
    }

    public IEnumerator Attack(float length)
    {
        float time = 0;
        var armLrotate = Quaternion.LookRotation((player.transform.position + Vector3.up * 1.0f) - armL[1].transform.position, Vector3.up) * Quaternion.Euler(-90, 0, 0);
        var armRrotate = Quaternion.LookRotation((player.transform.position + Vector3.up * 1.0f) - armR[1].transform.position, Vector3.up) * Quaternion.Euler(-90, 0, 0);
        var startRotateL = armL[1].rotation;
        var startRotateR = armR[1].rotation;

        while (time < 1.0f)
        {
            armL[1].rotation = Quaternion.Lerp(startRotateL, armLrotate, time / 1.0f);
            armR[1].rotation = Quaternion.Lerp(startRotateR, armRrotate, time / 1.0f);
            time += Time.deltaTime;
            yield return null;
        }
        
        time = 0;
        float shotCoolDown = 0;
        while (time <= length)
        {
            time += Time.deltaTime;
            gatlingRotate += Time.deltaTime * gatlingRotateSpeed;
            shotCoolDown += Time.deltaTime;
            if (gatlingRotate > 180f) gatlingRotate = -180f;
            armL[2].localRotation = Quaternion.Euler(0, -gatlingRotate, 0);
            armR[2].localRotation = Quaternion.Euler(0, gatlingRotate, 0);
            if (shotCoolDown >= shotInterval)
            {
                shotCoolDown = 0;
                Shot();
                PlayerDamage(2);
            }
            yield return null;
        }
        
        armLrotate = armL[1].localRotation;
        armRrotate = armR[1].localRotation;
        time = 0;
        while (time < 1.0f)
        {
            armL[1].localRotation = Quaternion.Lerp(armLrotate, Quaternion.identity, time / 1.0f);
            armR[1].localRotation = Quaternion.Lerp(armRrotate, Quaternion.identity, time / 1.0f);
            time += Time.deltaTime;
            yield return null;
        }
    }

    private void Shot()
    {
        sePlayer.PlaySound(4);
        Instantiate(bullet, muzzleL.transform.position, Quaternion.LookRotation(-muzzleL.transform.up + muzzleL.transform.right * Random.Range(0.01f, 0.03f) + muzzleL.transform.forward * Random.Range(-0.01f, 0.01f)));
        if (armR[0].gameObject.activeSelf) Instantiate(bullet, muzzleR.transform.position, Quaternion.LookRotation(-muzzleR.transform.up + muzzleR.transform.right * Random.Range(-0.03f, -0.01f) + muzzleR.transform.forward * Random.Range(-0.01f, 0.01f)));
    }

    public void ShotBossGun(float frame)
    {
        StartCoroutine(ShotGun(frame / 60f));
    }
    
    private IEnumerator ShotGun(float length)
    {
        foreach (var gun in bossGuns)
        {
            gun.randomRange = bossGunRandomRange;
            gun.shotDamage = bossGunShotDamage;
        }
        float endTime = Time.time + length;
        float shotCoolDown = 0;
        while (Time.time <= endTime)
        {
            shotCoolDown += Time.deltaTime;
            if (shotCoolDown >= shotIntervalGun)
                foreach (var gun in bossGuns)
                {
                    sePlayer.PlaySound(5);
                    gun.ShotBullet();
                    yield return new WaitForSeconds(0.1f);
                }
            yield return null;
        }
    }

    public void BossTargetDisable()
    {
        foreach (var target in bossTargets)
        {
            target.SetEnable(false);
        }
    }

    /// <summary>
    /// 右のガトリングを切り離して，力を加える
    /// </summary>
    public void GatlingBreak()
    {
        armR[0].gameObject.SetActive(false);
        var rb = armR2.GetComponent<Rigidbody>();
        armR2.transform.parent = null;
        armR2.SetActive(true);
        rb.AddForce(this.transform.right * 20 + Vector3.up * 5, ForceMode.Impulse);

        StartCoroutine(GatlingDestroy());
    }

    /// <summary>
    /// 分離したガトリングを削除する
    /// </summary>
    /// <returns></returns>
    private IEnumerator GatlingDestroy()
    {
        yield return new WaitForSeconds(5);
        armR2.GetComponent<CapsuleCollider>().isTrigger = true;
        yield return new WaitForSeconds(2);
        armR2.SetActive(false);
    }

    /// <summary>
    /// プレイヤーにダメージを与える
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void PlayerDamage(float damage)
    {
        if (player.CompareTag("Player")) player.Hit(damage);
    }
    
    /// <summary>
    /// スーパーアーマーの切り替え（タイムラインからの呼び出し用）
    /// </summary>
    /// <param name="enable">1-true / 0-false</param>
    public void SetSuperArmor(int enable)
    {
        timelineManager.superArmor = (enable != 0);
    }
}
