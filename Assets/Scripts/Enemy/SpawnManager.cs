using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;


public class SpawnManager : MonoBehaviour
{
    [Header("Class")]
    [SerializeField] private Player player;
    [SerializeField] private ScoreManager scoreManager;

    [Space(5), Header("Enemy")]
    [SerializeField] private Enemy1 enemy1;
    [SerializeField] private Enemy2 enemy2;

    [Space(5), Header("Spawning")]
    [SerializeField] private TimelineAsset[] timelines;
    [SerializeField] private TimelineAsset[] timelines2;

    [Space(5), Header("Others")]
    [SerializeField] private RectTransform rootUI;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private PlayableDirector playertimeline;

    private IEnumerator DisplayWave(string msg)
    {
        waveText.text = msg;
        Color textColor = waveText.color;
        float time = 0f;
        float displayTime = 0.5f;
        float fadeTime = 0.5f; // 表示が消えるまでの時間[秒] 適宜変更して
        while (time <= displayTime)
        {
            textColor.a = time / fadeTime;
            waveText.color = textColor;
            time += Time.deltaTime;
            yield return null;
        }
        time = 0;
        textColor.a = 1;
        waveText.color = textColor;
        yield return new WaitForSeconds(2.0f);
        while (time <= fadeTime)
        {
            textColor.a = 1 - (time / fadeTime);
            waveText.color = textColor;
            time += Time.deltaTime;
            yield return null;
        }
        textColor.a = 0;
        waveText.color = textColor;
    }
    
    public int enemyCount = 0;

    private void Start()
    {
        Color textColor = waveText.color;
        textColor.a = 0;
        waveText.color = textColor;
        StartCoroutine(Spawning());
    }


    private IEnumerator Spawning()
    {
        StartCoroutine(DisplayWave("Wave 0 / 3"));
        yield return new WaitForSeconds(3.0f);
        yield break;

        
        //=======================================WAVE 0
        Enemy1Spawn(11);
        while (enemyCount > 0) yield return null;
        Enemy2Spawn(0);
        while (enemyCount > 0) yield return null;
        Enemy1Spawn(10);
        yield return new WaitForSeconds(5.0f);
        Enemy2Spawn(2);
        while (enemyCount > 0) yield return null;
        Enemy2Spawn(3);
        yield return new WaitForSeconds(5.0f);
        Enemy1Spawn(9);
        Enemy1Spawn(4);
        while (enemyCount > 0) yield return null;

        StartCoroutine(DisplayWave("Wave 1 / 3"));
        yield return new WaitForSeconds(3.0f);
        //=======================================WAVE 1
        Enemy1Spawn(1);
        Enemy1Spawn(6);
        yield return new WaitForSeconds(5.0f);
        Enemy1Spawn(8);
        Enemy2Spawn(3);
        while (enemyCount > 0) yield return null;
        Enemy1Spawn(2);
        Enemy1Spawn(7);
        Enemy2Spawn(3);
        while (enemyCount > 0) yield return null;
        Enemy1Spawn(5);
        Enemy2Spawn(1);
        while (enemyCount > 0) yield return null;

        StartCoroutine(DisplayWave("Wave 2 / 3"));
        yield return new WaitForSeconds(3.0f);
        //=======================================WAVE 2
        PlayerMoveStart();
        Enemy2Spawn(7);
        Enemy1Spawn(12);
        Enemy1Spawn(14);
        while (enemyCount > 1) yield return null;
        Enemy1Spawn(13);
        Enemy2Spawn(5);
        while (enemyCount > 1) yield return null;
        Enemy1Spawn(0);
        Enemy2Spawn(6);
        while (enemyCount > 1) yield return null;
        Enemy1Spawn(2);
        Enemy1Spawn(7);
        yield return new WaitForSeconds(0.5f);
        Enemy1Spawn(3);
        yield return new WaitForSeconds(0.5f);
        Enemy1Spawn(14);
        while (enemyCount > 0) yield return null;

        StartCoroutine(DisplayWave("Wave 3 / 3"));
        yield return new WaitForSeconds(3.0f);
        //=======================================WAVE 3
        PlayerMoveStart();
        Enemy1Spawn(14);
        Enemy1Spawn(11);
        Enemy1Spawn(6);

        while (enemyCount > 0) yield return null; // 敵が0になるまで待つ
        Enemy1Spawn(4);
        Enemy1Spawn(5);
        yield return new WaitForSeconds(5.0f);
        Enemy1Spawn(0);
        Enemy2Spawn(3);
        while (enemyCount > 1) yield return null;
        Enemy1Spawn(10);
        Enemy1Spawn(11);
        yield return new WaitForSeconds(5.0f);
        Enemy1Spawn(13);
        while (enemyCount > 0) yield return null;

        StartCoroutine(DisplayWave("Well Done!"));
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(DisplayWave("Next Stage..."));
        PlayerMoveStart();
    }

    private void Enemy1Spawn(int spawnPoint)
    {
        var enemy = Instantiate(enemy1);
        var director = enemy.GetComponent<PlayableDirector>();
        enemy.spawnManager = this;
        enemy.player = player;
        enemy.rootUI = rootUI;
        enemy.scoreManager = scoreManager;
        enemy.timelineCount = spawnPoint;
        director.playableAsset = timelines[spawnPoint];
        director.Play();
        enemyCount++;
    }

    private void Enemy2Spawn(int spawnPoint)
    {
        var enemy = Instantiate(enemy2);
        var director = enemy.GetComponent<PlayableDirector>();
        enemy.spawnManager = this;
        enemy.player = player;
        enemy.rootUI = rootUI;
        enemy.scoreManager = scoreManager;
        enemy.timelineCount = spawnPoint;
        director.playableAsset = timelines2[spawnPoint];
        director.Play();
        enemyCount++;
    }
    

    public void PlayerMoveStart()
    {
        playertimeline.Play();
    }

    public void PlayerMoveStop()
    {
        // playertimeline.Pause();
    }
}


/*
        Enemy1Spawn(0);
        Enemy1Spawn(7);
        yield return new WaitForSeconds(1.5f);
        Enemy1Spawn(5);
        Enemy1Spawn(6);
        while (enemyCount > 0) yield return null;//敵が0になるまで待つ
        
        
            Enemy1Spawn(1);
            Enemy1Spawn(6);
            Enemy1Spawn(8);
            yield return new WaitForSeconds(1.5f);
            Enemy1Spawn(0);
            while (enemyCount > 0) yield return null;//敵が0になるまで待つ
            Enemy1Spawn(2);
            Enemy1Spawn(3);
            Enemy1Spawn(7);
            yield return new WaitForSeconds(1.5f);
            Enemy1Spawn(5);
            while (enemyCount > 0) yield return null;//敵が0になるまで待つ
            Enemy1Spawn(1);
            yield return new WaitForSeconds(0.5f);
            Enemy1Spawn(2);
            yield return new WaitForSeconds(0.5f);
            Enemy1Spawn(3);
            yield return new WaitForSeconds(0.5f);
            Enemy1Spawn(8);
            while (enemyCount > 0) yield return null;//敵が0になるまで待つ
        }*/