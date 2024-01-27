using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public static bool easyReload;

    [SerializeField] private Arduino arduino;

    private void Update()
    {
        easyReload = arduino.magazine;
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void Config()
    {
        SceneManager.LoadScene("Debug");
    }

    public void End()
    {
        Debug.Log("End!");
    }
}
