using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

    public GameObject SplashScreen;

    void Awake()
    {
        if (SplashScreen != null)
        {
            SplashScreen.SetActive(true);
        }
    }

    // Use this for initialization
    void Start () {
        
    }
    

    public void OnStartGame()
    {
        SceneManager.LoadScene("main");   
    }
}
