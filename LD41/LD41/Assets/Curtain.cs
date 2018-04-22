using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curtain : MonoBehaviour {

    public enum State
    {
        Idle,
        Closing,
        Opening
    }

    public State CurtainState = State.Idle;
    
    public float Rate = 3.0f;

    public float StopDistance = 5.0f;

    private float StartPosX = 0.0f;

    public AudioSource Source;
    public AudioClip Sound;

    void Awake()
    {
        
    }

	// Use this for initialization
	void Start () {
        StartPosX = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
	
        if (CurtainState == State.Closing)
        {
            Vector3 pos = transform.position;
            pos.x += Rate * Time.deltaTime;

            transform.position = pos;

            float delta = Mathf.Abs(pos.x - StartPosX);

            if (delta > StopDistance)
            {
                CurtainState = State.Idle;

                Source.Stop();
            }
        }
        else if (CurtainState == State.Opening)
        {
            Vector3 pos = transform.position;
            pos.x += -Rate * Time.deltaTime;

            transform.position = pos;

            float delta = Mathf.Abs(pos.x - StartPosX);

            if (delta <= 0.1f)
            {
                pos.x = StartPosX;
                transform.position = pos;
                CurtainState = State.Idle;

                Source.Stop();
            }
        }	
	}

    public void SetState(Curtain.State state)
    {
        if (CurtainState != State.Idle)
            return;

        Source.Play();

        CurtainState = state;
    }
}
