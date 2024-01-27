using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DebugManager : MonoBehaviour
{
    [SerializeField] private Arduino arduino;
    [SerializeField] private Setting setting;
    [SerializeField] private RectTransform pointer;
    [SerializeField] private TextMeshProUGUI pitchOffsetText;
    [SerializeField] private TextMeshProUGUI pointerOffsetText;
    [SerializeField] private TextMeshProUGUI alignText;

    private Vector3 pointerPos;
    private float pitchOffset = 0f;

    private void Start()
    {
        StartCoroutine(AlignOffset());
    }
    
    private void Update()
    {
        var yaw = arduino.yaw;
        if (yaw > 180) yaw -= 360;
        pointerPos = new Vector3(Mathf.Atan(yaw * Mathf.Deg2Rad) * Setting.pointerOffset, Mathf.Atan((arduino.pitch + pitchOffset) * Mathf.Deg2Rad) * Setting.pointerOffset, 0) + new Vector3(Screen.width, Screen.height) / 2;
    }

    private IEnumerator AlignOffset()
    {
        yield return new WaitForSeconds(0.5f);
        Setting.pitchOffset = 0f;
        pointer.gameObject.SetActive(false);
        alignText.text = "Pitch";
        pitchOffsetText.text = "Pitch : " + pitchOffset.ToString("f2");
        pointerOffsetText.text = "Pointer : " + Setting.pointerOffset.ToString("f2");

        // トリガー入力中は停止
        while (arduino.trigger) yield return null;
        yield return new WaitForSeconds(0.5f);

        // 画面を撃って大まかな角度を出す
        // その時の回転角が0になるようにオフセットを出せば良い
        while (true)
        {
            // デバッグキーで元に戻る
            if (arduino.debug1 || arduino.debug2) ReturnTitle();
            if (arduino.trigger) break;
            yield return null;
        }
        pitchOffset = -arduino.pitch;
        Setting.pitchOffset = pitchOffset;
        
        // トリガー入力中は停止
        while (arduino.trigger) yield return null;

        // 詳細位置を設定 ( trigger : +, hide : - )
        pointer.gameObject.SetActive(true);
        while (true)
        {
            pointer.position = Vector3.Scale(pointerPos, new Vector3(0, 1, 0)) + new Vector3(Screen.width / 2, 0, 0);
            if (arduino.trigger) pitchOffset += Time.deltaTime * 2;
            else if (arduino.hide) pitchOffset -= Time.deltaTime * 2;
            if (arduino.debug1 || arduino.debug2) break;
            pitchOffsetText.text = "Pitch : " + pitchOffset.ToString("f2");
            yield return null;
        }
        Setting.pitchOffset = pitchOffset;
        
        // デバッグボタン入力中は停止
        while (arduino.debug1 || arduino.debug2) yield return null;
        yield return new WaitForSeconds(0.5f);
        alignText.text = "Pointer";
        
        // 回転オフセットを設定 ( trigger : +, hide : - )
        while (true)
        {
            pointer.position = pointerPos;
            if (arduino.trigger) Setting.pointerOffset += Time.deltaTime * 200;
            else if (arduino.hide) Setting.pointerOffset -= Time.deltaTime * 200;
            if (arduino.debug1 || arduino.debug2) break;
            pointerOffsetText.text = "Pointer : " + Setting.pointerOffset.ToString("f2");
            yield return null;
        }
        
        setting.Save();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Title");
    }

    private void ReturnTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
