using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private TimelineAsset[] phases;
    [SerializeField] private int phaseCount = 0;
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private Boss boss;
    [SerializeField] private Player player;
    [SerializeField] private MissileManager missileManager;
    [SerializeField] private GameObject bossModel;
    [SerializeField] private GameObject bossBGM;
    [SerializeField] private GameObject zakoBGM;

    public bool superArmor = false;
        
    private bool isStop = false;

    private void Start()
    {
        BossStart();
    }

    public void BossStart()
    {
        bossBGM.SetActive(true);
        zakoBGM.SetActive(false);
        player.allowShot = false;
        bossModel.SetActive(true);
        NextPhase();
    }
        
    public void NextPhase()
    {
        superArmor = false;
        player.allowShot = true;
        director.Pause();
        phaseCount++;
        if (phaseCount % 2 == 0) player.allowShot = false;
        director.playableAsset = phases[phaseCount];
        director.time = 0;
        director.Play();
    }

    public void StopTimeline()
    {
        director.Pause();
        isStop = true;
        boss.BossTargetDisable();
    }

    public void DamageMotion()
    {
        StartCoroutine(DamageMotionCoroutine());
    }

    private IEnumerator DamageMotionCoroutine()
    {
        director.Pause();
        bossAnimator.Play("GatlingDamage_R");
        yield return new WaitForSeconds(2.6f);
        if (!isStop) director.Play();
    }

    public void Attack(float len)
    {
        StartCoroutine(AttackCoroutine(len));
    }

    private IEnumerator AttackCoroutine(float len)
    {
        superArmor = true;
        director.Pause();
        boss.StartCoroutine("Attack", len);
        yield return new WaitForSeconds(2.2f + len);
        superArmor = false;
        if (!isStop) director.Play();
    }

    public void ShotMissile(int count)
    {
        StartCoroutine(ShotMissileCoroutine(count));
    }
    
    private IEnumerator ShotMissileCoroutine(int count)
    {
        superArmor = true;
        director.Pause();
        missileManager.ShotMissile(count);
        yield return new WaitForSecondsRealtime(10f);
        superArmor = false;
        if (!isStop) director.Play();
    }

    public void TimelineLoop(float frame)
    {
        director.time = frame / 60f;
    }
}
