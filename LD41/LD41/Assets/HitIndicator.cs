using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitIndicator : MonoBehaviour
{
    public float DecayRate = 0.1f;


    private float AlphaValue = 0.0f;

    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        if (AlphaValue > 0.0f)
        {
            AlphaValue -= Time.deltaTime * DecayRate;
        }

        AlphaValue = Mathf.Max(0.0f, AlphaValue);

        Image image = GetComponent<Image>();
        if (image != null)
        {
            Color color = image.color;
            color.a = AlphaValue;

            image.color = color;
        }
    }


    public void GetHit()
    {
        AlphaValue = 0.8f;
    }
}
