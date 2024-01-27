using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PointerController : MonoBehaviour
{
    [SerializeField] private Arduino arduino;
    [SerializeField] private RectTransform pointer;
    [SerializeField] private Vector3 pointerPos;
    [SerializeField] private TextMeshProUGUI[] texts;
    
    [Header("Color")]
    [SerializeField] private Color normalColor;
    [SerializeField] private Color highlightColor;
    
    private bool isPush;

    private void Update()
    {
        if (!Setting.isArduino)
        {
            pointerPos = Input.mousePosition - new Vector3(Screen.width, Screen.height) / 2;
            pointer.localPosition = pointerPos;
        }
        else
        {
            var yaw = arduino.yaw;
            if (yaw > 180) yaw -= 360;
            pointerPos = new Vector3(Mathf.Atan(yaw * Mathf.Deg2Rad) * Setting.pointerOffset, Mathf.Atan(arduino.pitch * Mathf.Deg2Rad) * Setting.pointerOffset, 0) + new Vector3(Screen.width, Screen.height) / 2;
            pointer.position = pointerPos;

            if (!arduino.trigger) isPush = false;
        }
        
        var pointData = new PointerEventData(EventSystem.current);
        pointData.position = pointerPos;

        List<RaycastResult> RayResult = new List<RaycastResult>();

        EventSystem.current.RaycastAll(pointData, RayResult);

        foreach (RaycastResult result in RayResult)
        {
            if (result.gameObject.CompareTag("Text"))
            {
                var resultName = result.gameObject.name;
                foreach (var text in texts) text.color = (text.name == resultName) ? highlightColor : normalColor;
            }
            if (result.gameObject.CompareTag("Button"))
            {
                if (Input.GetKeyDown(KeyCode.Space) || (Setting.isArduino && arduino.trigger && !isPush))
                {
                    result.gameObject.GetComponent<Button>().onClick.Invoke();
                    isPush = true;
                }
            }
        }
    }
}
