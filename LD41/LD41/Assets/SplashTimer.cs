using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashTimer : MonoBehaviour {

    private float Timer = 3.0f;

    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        Timer -= Time.deltaTime;
        
        if (Timer <= 0.0f)
        {
            gameObject.SetActive(false);
        }	
    }
}
