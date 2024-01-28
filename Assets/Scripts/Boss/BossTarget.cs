using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BossTarget : MonoBehaviour
{
    [Header("Class")]
    [SerializeField] private Boss boss;
    [SerializeField] private TimelineManager timelineManager;
    [SerializeField] private ScoreManager scoreManager;

    [Space(5), Header("Prefab")]
    [SerializeField] private BossTargetUI targetUI;
    
    [Space(5), Header("Assign")]
    [SerializeField] private RectTransform rootUI;
    
    [Space(5), Header("Setting")]
    [SerializeField] private float maxHP;
    [SerializeField] private int score;
    public int phase;

    [Space(5), Header("Event")]
    [SerializeField] private UnityEvent DestoryEvent;

    public float hp { get; private set; }
    private BossTargetUI target;
    private BoxCollider col;
    
    private void Awake()
    {
        hp = maxHP;
    }

    private void Start()
    {
        col = GetComponent<BoxCollider>();
        target = Instantiate(targetUI, rootUI.GetChild(0));
        target.bossTarget = this;
        SetEnable(phase <= 0);
    }

    public void Damage(float damage)
    {
        hp -= damage;
        if (timelineManager.superArmor && hp <= 0) hp = 1;
        hp = Mathf.Clamp(hp, 0, maxHP);
        boss.Damage();
        target.HPbarUpdate(hp / maxHP);
        if (hp <= 0)
        {
            SetEnable(false);
            DestoryEvent.Invoke();
            scoreManager.AddScore(score);
        }
    }

    public void SetEnable(bool value)
    {
        col.enabled = value;
        this.gameObject.tag = value ? "Boss" : "Untagged";
        target.gameObject.SetActive(value);
    }
}
