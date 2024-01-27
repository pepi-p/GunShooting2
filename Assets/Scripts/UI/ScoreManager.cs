using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    
    public int shotCount { get; set; }
    public int shotHitCount { get; set; }
    
    public int score { get; private set; }
    private float displayScore = 0;
    
    private void Update()
    {
        if (displayScore < score) displayScore = Mathf.Lerp(displayScore, score, Time.deltaTime * 20);
        if (displayScore > score) displayScore = Mathf.Lerp(displayScore, score, Time.deltaTime * 20);

        uiManager.DisplayScore(displayScore);
    }
    
    public void AddScore(int value)
    {
        score += value;
        if (score < 0) score = 0;
        if (value < 5) displayScore = score;
        uiManager.DisplayScore(displayScore);
    }
}
