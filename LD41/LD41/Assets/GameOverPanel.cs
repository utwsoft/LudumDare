using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour {

    public GameObject GameOverText;
    public GameObject RestartButton;

    public AudioSource Audio;
    public AudioClip Sound;


    private float curTime = 0.0f;

    public float GameOverRevealTime = 3.0f;

    public float RestartButtonRevealTime = 3.7f;

    void Awake()
    {
        GameOverText.SetActive(false);
        RestartButton.SetActive(false);
    }

    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        curTime += Time.deltaTime;

        if (curTime > GameOverRevealTime)
        {

            if (!GameOverText.activeSelf)
            {
                if (Audio != null && Sound != null)
                {
                    Audio.PlayOneShot(Sound);
                }

                GameOverText.SetActive(true);
            }

        }

        if (curTime > RestartButtonRevealTime)
        {
            RestartButton.SetActive(true);
        }


    }

    public void ResetPanel()
    {
        curTime = 0.0f;

        GameOverText.SetActive(false);
        RestartButton.SetActive(false);
    }
}
