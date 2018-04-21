using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTimer : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    public Sprite[] Sprites;

    float curTime = 0.0f;
    public float FrameTime = 0.1875f;

    int frameIndex = 0;

	// Use this for initialization
	void Start () {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        SetSprite(Sprites[0]);
	}
	
    
	// Update is called once per frame
	void Update () {

        curTime += Time.deltaTime;

        if (curTime > FrameTime)
        {
            frameIndex++;
            frameIndex %= Sprites.Length;

            curTime = 0.0f;

            SetSprite(Sprites[frameIndex]);
        }
	}

    void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
