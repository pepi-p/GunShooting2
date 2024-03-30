using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTarget : MonoBehaviour
{
    public Transform target;

    private Camera cam;
    private RectTransform rootUI;
    private RectTransform thisUI;
    
    private void Start()
    {
        cam = Camera.main;
        thisUI = GetComponent<RectTransform>();
        rootUI = this.transform.root.GetComponent<RectTransform>();
    }

    private void Update()
    {
        var targetScreenPos = cam.WorldToScreenPoint(target.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rootUI, targetScreenPos, null, out var uiPos);
        thisUI.localPosition = uiPos;
    }
}
