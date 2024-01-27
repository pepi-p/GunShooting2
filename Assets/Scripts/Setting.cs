using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Setting : MonoBehaviour
{
    public static float pointerOffset;
    public static string comPort;
    public static bool isArduino;
    public static float pitchOffset;

    private void Awake()
    {
        Load();
    }

    private void Load()
    {
        string path = Application.dataPath + @"\setting.txt";

        if (!File.Exists(path))
        {
            using (File.Create(path)) {}

            pointerOffset = 1800;
            comPort = "COM3";
            isArduino = false;
            pitchOffset = 0f;
            Save();
        }
        else
        {
            var data = File.ReadAllLines(path);
            comPort = data[0];
            pointerOffset = float.Parse(data[1]);
            isArduino = bool.Parse(data[2]);
            pitchOffset = float.Parse(data[3]);
        }
    }

    public void Save()
    {
        string path = Application.dataPath + @"\setting.txt";
        string[] data = new string[4];

        data[0] = comPort;
        data[1] = pointerOffset.ToString();
        data[2] = isArduino.ToString();
        data[3] = pitchOffset.ToString();
            
        File.WriteAllLines(path, data);
    }
}
