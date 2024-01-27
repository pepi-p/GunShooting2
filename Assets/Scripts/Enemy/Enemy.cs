using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SocialPlatforms.Impl;

[Serializable]
public class Enemy : MonoBehaviour
{
    [Header("Class")]
    public SpawnManager spawnManager;
    public ScoreManager scoreManager;
    public Player player;
    
    [Space(5), Header("Prefab")]
    [SerializeField] protected EnemyBullet bullet;
    [SerializeField] private GameObject explosionParticle;
    [SerializeField] private EnemyTarget enemyTarget;
    
    [Space(5), Header("Setting")]
    [SerializeField] protected float shotDamage;
    [SerializeField] private Vector3 explosionOffset;
    [SerializeField] private Vector3 uiDisplayOffset;
    [SerializeField] private Vector3[] eventFrame;
    public float hp;
    [SerializeField] private int score;
    
    [Space(5), Header("Assign")]
    [SerializeField] protected GameObject shotPos;
    public PlayableDirector timeline;
    public RectTransform rootUI;

    public int timelineCount { get; set; }
    private Animator animator;
    protected float maxHP = 1;
    protected EnemyTarget targetUI;

    private void Start()
    {
        animator = GetComponent<Animator>();
        transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
        maxHP = hp;
        targetUI = Instantiate(enemyTarget, rootUI.GetChild(0));
        targetUI.targetPos = this.transform;
        targetUI.uiDisplayOffset = uiDisplayOffset;
    }

    private void FixedUpdate()
    {
        targetUI.AttackBar(Mathf.Clamp01((float)((timeline.time * 60 - eventFrame[timelineCount].x) / (eventFrame[timelineCount].y - eventFrame[timelineCount].x))));
    }

    public void Hit(float damage)
    {
        if (hp <= 0) return;
        hp -= damage;
        targetUI.Damage(hp / maxHP);
        if (hp <= 0)
        {
            timeline.Pause();
            if (eventFrame[timelineCount].z > 0)
            {
                timeline.time = eventFrame[timelineCount].z / 60f;
                timeline.Play();
                scoreManager.AddScore(score);
            }
            else animator.Play("Destroy");
        }
    }

    // アニメーションからの呼び出し
    public void ShotBullet()
    {
        Instantiate(bullet, shotPos.transform.position, Quaternion.LookRotation(player.transform.position + Vector3.up * 1.2f - shotPos.transform.position));
        if(player.CompareTag("Player")) player.Hit(shotDamage);
    }

    public void Explosion()
    {
        Instantiate(explosionParticle, this.transform.position + explosionOffset, Quaternion.identity);
    }

    public void DestroyEnemy()
    {
        spawnManager.enemyCount--;
        Destroy(this.gameObject);
    }
    // アニメーションからの呼び出し - end
}