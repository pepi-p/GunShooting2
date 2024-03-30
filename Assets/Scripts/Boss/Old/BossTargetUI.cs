using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossTargetUI : MonoBehaviour
{
    [Header("Class")]
    public BossTarget bossTarget;
    
    [Space(5), Header("Assign")]
    [SerializeField] private RectTransform targetUI;
    [SerializeField] private Image hpBar;
    
    private Transform targetPos;
    private RectTransform rootUI;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        rootUI = targetUI.root.GetChild(0).GetComponent<RectTransform>();
        targetPos = bossTarget.transform;
    }

    private void Update()
    {
        var targetScreenPos = cam.WorldToScreenPoint(targetPos.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rootUI, targetScreenPos, null, out var uiPos);
        targetUI.localPosition = uiPos;
    }

    public void HPbarUpdate(float hpRate)
    {
        hpBar.fillAmount = hpRate;
    }
}
