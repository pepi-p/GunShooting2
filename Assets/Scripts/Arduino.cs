using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arduino : MonoBehaviour
{
    public SerialHandler serialHandler;

    public float yaw = 0;
    public float pitch = 0;
    public float roll = 0;
    public bool trigger;
    public bool hide;
    public bool debug1;
    public bool debug2;
    public bool magazine;

    void Start()
    {
        //信号を受信したときに、そのメッセージの処理を行う
        serialHandler.OnDataReceived += OnDataReceived;
    }
    
    // true : モーターを止める, false : モーターを動かす
    public void MotorStop(bool value)
    {
        serialHandler.Write(value ? "a" : "b");
    }

    //受信した信号(message)に対する処理
    void OnDataReceived(string message)
    {
        var data = message.Split(new string[] { "\t" }, System.StringSplitOptions.None);
        try
        {
            roll = float.Parse(data[0]);
            var pitch_raw = float.Parse(data[1]);
            yaw = float.Parse(data[2]);
            trigger = (int.Parse(data[3]) != 0);
            hide = (int.Parse(data[4]) != 0);
            debug1 = (int.Parse(data[5]) != 0);
            debug2 = (int.Parse(data[6]) != 0);
            magazine = (int.Parse(data[7]) != 0);
            pitch = pitch_raw + Setting.pitchOffset;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message); //エラーを表示
        }
    }

    
}
