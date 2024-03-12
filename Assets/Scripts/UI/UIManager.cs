using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤー以外のUIの表示
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] timer0;
    [SerializeField] private GameObject[] timer1;
    [SerializeField] private GameObject[] timer2;
    [SerializeField] private GameObject[] timer3;
    [SerializeField] private GameObject[] ammo0;
    [SerializeField] private GameObject[] ammo1;
    [SerializeField] private GameObject ammo100;
    [SerializeField] private GameObject[] score0;
    [SerializeField] private GameObject[] score1;
    [SerializeField] private GameObject[] score2;
    [SerializeField] private GameObject[] score3;
    [SerializeField] private GameObject[] score4;
    [SerializeField] private GameObject[] score5;

    private float time;

    private void Start()
    {
        Cursor.visible = false;
        StartCoroutine(CountTimer());
    }

    private void SetValue(GameObject[] obj, int value)
    {
        for(int i = 0; i < 10; i++) obj[i].SetActive(i == value);
    }

    private IEnumerator CountTimer()
    {
        while(time >= 0)
        {
            // d0 d1 : d2 d3
            int d0 = ((int)(time / 60)) / 10; // 0桁目
            int d1 = ((int)(time / 60)) % 10; // 1桁目
            int d2 = ((int)(time % 60)) / 10; // 2桁目
            int d3 = ((int)(time % 60)) % 10; // 3桁目
            SetValue(timer0, d0);
            SetValue(timer1, d1);
            SetValue(timer2, d2);
            SetValue(timer3, d3);
            yield return new WaitForSeconds(1);
            time++;
        }
    }

    public void DisplayScore(float score)
    {
        var displayScoreInt = Mathf.RoundToInt(score);
        // d0 d1 d2 d3 d4 d5
        int d0 = displayScoreInt / 100000;       // 0桁目 (左から)
        int d1 = (displayScoreInt / 10000) % 10; // 1桁目
        int d2 = (displayScoreInt / 1000) % 10;  // 2桁目
        int d3 = (displayScoreInt / 100) % 10;   // 3桁目
        int d4 = (displayScoreInt / 10) % 10;    // 4桁目
        int d5 = displayScoreInt % 10;           // 5桁目
        SetValue(score0, d0);
        SetValue(score1, d1);
        SetValue(score2, d2);
        SetValue(score3, d3);
        SetValue(score4, d4);
        SetValue(score5, d5);
    }
}
