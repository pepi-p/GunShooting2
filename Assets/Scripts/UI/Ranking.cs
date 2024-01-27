using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Linq;

public class Ranking : MonoBehaviour
{
    public static string[] names = new string[6]{ "-", "-", "-", "-", "-", "-" };
    public static int[] waves = new int[6]{ -1, -1, -1, -1, -1, -1 };

    [SerializeField] private TextMeshProUGUI[] nameText;
    [SerializeField] private TextMeshProUGUI[] waveText;

    private void Start()
    {
        if(waves.Sum() == -6) Load();

        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                if(waves[j + 1] > waves[j])
                {
                    var tmpWav = waves[j + 1];
                    var tmpNam = names[j + 1];
                    waves[j + 1] = waves[j];
                    names[j + 1] = names[j];
                    waves[j] = tmpWav;
                    names[j] = tmpNam;
                }
            }
        }

        Save();

        DisplayUpdate();
    }

    private void DisplayUpdate()
    {
        for(int i = 0; i < nameText.Length; i++)
        {
            nameText[i].text = names[i];
            if (waves[i] != -1) waveText[i].text = waves[i].ToString("D6");
            else waveText[i].text = 0.ToString("D6");
        }
    }

    private void Load()
    {
        string path = Application.dataPath + @"\ranking.txt";

        if (!File.Exists(path))
        {
			using (File.Create(path)) {}
            Save();
		}
        else
        {
            var data = File.ReadAllLines(path);
            for(int i = 0; i < 5; i++)
            {
                names[i] = data[2 * i];
                waves[i] = int.Parse(data[2 * i + 1]);
            }
        }
    }

    private void Save()
    {
        string path = Application.dataPath + @"\ranking.txt";
        string[] data = new string[10];

        for(int i = 0; i < 5; i++)
        {
            data[2 * i] = names[i];
            data[2 * i + 1] = waves[i].ToString();
        }

        File.WriteAllLines(path, data);
    }
}
