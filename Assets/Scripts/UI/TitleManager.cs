using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public static bool easyReload;

    [SerializeField] private Arduino arduino;
    [SerializeField] private SerialHandler serialHandler;

    private void Start()
    {
        StartCoroutine(GunReset());
    }

    private IEnumerator GunReset()
    {
        yield return new WaitForSeconds(2);
        serialHandler.Write("100\n");
        yield return new WaitForSeconds(0.5f);
        serialHandler.Write("201\n");
    }

    private void Update()
    {
        // easyReload = arduino.magazine;
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
