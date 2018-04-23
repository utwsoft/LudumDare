using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointDisplayAnimator : MonoBehaviour {

    private float curTime = 0.0f;

    public float ExpireTime = 2.0f;

    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        curTime += Time.deltaTime;

        Vector3 pos = transform.position;
        pos.y += curTime * 2.0f;
        transform.position = pos;


        Text txt = GetComponent<Text>();
        Outline outline = GetComponent<Outline>();

        Color color = txt.color;
        Color outColor = outline.effectColor;
        if (curTime > 0.5f)
        {
            color.a = 1.0f - ((curTime - 1.0f) * 2.0f);
            txt.color = color;

            outColor.a = 1.0f - ((curTime - 1.0f) * 2.0f);
            outline.effectColor = outColor;
        }

        if (curTime > ExpireTime)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
