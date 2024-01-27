using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InputName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] resultNameText;
    
    private string resultName = null;
    private int count;
    
    public void EnterAlphabet(string ch)
    {
        if(count > 2) return;
        count++;
        resultName += ch;
        var texts = resultName.ToCharArray();
        for (int i = 0; i < count; i++)
        {
            resultNameText[i].text = texts[i].ToString();
        }
    }

    public void Delete()
    {
        if (count < 1) return;
        count--;
        resultName = resultName.Remove(count, 1);
        resultNameText[count].text = null;
    }

    public void Enter()
    {
        if (count == 0) return;
        Ranking.names[5] = resultName;
        SceneManager.LoadScene("Title");
    }
}
