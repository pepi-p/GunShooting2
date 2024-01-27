using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    [Header("Class")]
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private Player player;
    [SerializeField] private TimelineManager timelineManager;
    [SerializeField] private Arduino arduino;
    
    [Space(5), Header("ResultUI")]
    [SerializeField] private GameObject resultObject;
    [SerializeField] private TextMeshProUGUI noDamageText;
    [SerializeField] private Color enableColor;
    [SerializeField] private Color disableColor;
    [SerializeField] private TextMeshProUGUI returnTitleMsg;
    [SerializeField] private GameObject bar;
    [SerializeField] private GameObject[] indexTexts;
    
    [Space(5), Header("Result_Score")]
    [SerializeField] private TextMeshProUGUI baseScoreScoreText;
    [SerializeField] private TextMeshProUGUI clearTimeScoreText;
    [SerializeField] private TextMeshProUGUI hpRateScoreText;
    [SerializeField] private TextMeshProUGUI hitRateScoreText;
    [SerializeField] private TextMeshProUGUI noDamageScoreText;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    
    [Space(5), Header("Result_Value")]
    [SerializeField] private TextMeshProUGUI clearTimeValueText;
    [SerializeField] private TextMeshProUGUI hpRateValueText;
    [SerializeField] private TextMeshProUGUI hitRateValueText;

    [Space(5), Header("GamaOver")]
    [SerializeField] private GameObject gameover;
    [SerializeField] private GameObject returnTitleGameover;
    
    public bool isBossBattle { get; set; }
    
    private float startTime;
    private bool isDisplay;
    private bool isArduino;
    private bool isGameover;
    private bool allowSkip;
    
    private void Start()
    {
        startTime = Time.time;
        isArduino = arduino.gameObject.activeSelf;
    }

    private void Update()
    {
        if (allowSkip)
        {
            if ((isArduino && arduino.trigger && arduino.hide) || (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.Space)))
            {
                if (!isBossBattle) Time.timeScale = 1;
                if (!isGameover) SceneManager.LoadScene("InputName");
                else SceneManager.LoadScene("Title");
            }
        }
    }

    public void DisplayResult()
    {
        if(isDisplay) return;
        isDisplay = true;
        timelineManager.StopTimeline();
        var clearTime = Time.time - startTime;
        resultObject.SetActive(true);
        clearTimeValueText.text = (int)clearTime / 60 + ":" + ((int)clearTime % 60).ToString("00");
        hpRateValueText.text = (player.GetHPRate() * 100).ToString("f1") + " %";
        hitRateValueText.text = scoreManager.shotHitCount + " / " + scoreManager.shotCount;
        var isNodamage = (player.GetHPRate() >= 1);
        noDamageText.color = isNodamage ? enableColor : disableColor;
        noDamageScoreText.color = isNodamage ? enableColor : disableColor;
        var baseScore = scoreManager.score;
        var timeScore = GetClearTimeScore(clearTime);
        var hpRateScore = player.GetHPRate() * 100 * 500;
        var hitRateScore = ((float)scoreManager.shotHitCount / scoreManager.shotCount) * 20000;
        var nodamageScore = isNodamage ? 50000 : 0;
        baseScoreScoreText.text = baseScore.ToString();
        clearTimeScoreText.text = ((int)timeScore).ToString();
        hpRateScoreText.text = ((int)hpRateScore).ToString();
        hitRateScoreText.text = ((int)hitRateScore).ToString();
        noDamageScoreText.text = isNodamage ? 50000.ToString() : 0.ToString();
        var total = baseScore + timeScore + hpRateScore + hitRateScore + nodamageScore;
        totalScoreText.text = ((int)total).ToString();
        Ranking.waves[5] = (int)total;
        StartCoroutine(DelayDisplay());
    }

    private IEnumerator DelayDisplay()
    {
        baseScoreScoreText.gameObject.SetActive(true);
        indexTexts[0].SetActive(true);
        yield return new WaitForSeconds(0.4f);
        clearTimeScoreText.gameObject.SetActive(true);
        clearTimeValueText.gameObject.SetActive(true);
        indexTexts[1].SetActive(true);
        yield return new WaitForSeconds(0.4f);
        hpRateScoreText.gameObject.SetActive(true);
        hpRateValueText.gameObject.SetActive(true);
        indexTexts[2].SetActive(true);
        yield return new WaitForSeconds(0.4f);
        hitRateScoreText.gameObject.SetActive(true);
        hitRateValueText.gameObject.SetActive(true);
        indexTexts[3].SetActive(true);
        yield return new WaitForSeconds(0.4f);
        noDamageText.gameObject.SetActive(true);
        noDamageScoreText.gameObject.SetActive(true);
        indexTexts[4].SetActive(true);
        yield return new WaitForSeconds(0.4f);
        bar.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        totalScoreText.gameObject.SetActive(true);
        indexTexts[5].SetActive(true);
        allowSkip = true;
        yield return new WaitForSeconds(0.8f);
        returnTitleMsg.gameObject.SetActive(true);
    }

    private int GetClearTimeScore(float time)
    {
        if (time > 300) return 2500;
        else if (time > 270) return 5000;
        else if (time > 240) return 1000;
        else if (time > 210) return 20000;
        else return 30000;
    }

    public void DisplayGameOver()
    {
        if(isDisplay) return;
        isGameover = true;
        isDisplay = true;
        if (isBossBattle) timelineManager.StopTimeline();
        else Time.timeScale = 0;
        gameover.SetActive(true);
        StartCoroutine(DelayGameOver());
    }

    private IEnumerator DelayGameOver()
    {
        while (true)
        {
            returnTitleGameover.SetActive(false);
            yield return new WaitForSecondsRealtime(1f);
            allowSkip = true;
            returnTitleGameover.SetActive(true);
            yield return new WaitForSecondsRealtime(1f);
        }
    }
}
