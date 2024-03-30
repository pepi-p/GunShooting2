using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;

public class MissileManager : MonoBehaviour
{
    /*
    [SerializeField] private Player player;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private SerialHandler serialHandler;
    [SerializeField] private Missile missile;
    [SerializeField] private MissileTarget missileTarget;
    [SerializeField] private Transform shotPos;
    [SerializeField] private RectTransform canvas;
    [SerializeField] private SEPlayer sePlayer;

    [Header("Settings")]
    [SerializeField] private float timeScale = 0.1f;
    [SerializeField] private float slowTime = 5.0f;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z)) ShotMissile(4); // デバッグ用
    }
    
    public void ShotMissile(int count)
    {
        sePlayer.PlaySound(6);
        for (int i = 0; i < count; i++)
        {
            var theta = (20 * (i - ((count - 1) / 2f))) * Mathf.Deg2Rad;
            theta += Random.Range(-0.06f, 0.06f);
            var mis = Instantiate(missile, shotPos.position, Quaternion.LookRotation(shotPos.up * Mathf.Cos(theta) + shotPos.right * Mathf.Sin(theta)));
            var target = Instantiate(missileTarget, canvas);
            mis.Setup(player, target.gameObject, scoreManager);
            target.target = mis.transform;
        }

        StartCoroutine(QuickTimeEvent());
    }

    private IEnumerator QuickTimeEvent()
    {
        yield return new WaitForSeconds(1.2f);
        Time.timeScale = 0.1f;
        serialHandler.Write("303\n");
        yield return new WaitForSecondsRealtime(5.0f);
        Time.timeScale = 1.0f;
        serialHandler.Write("304\n");
    }
    */
}
