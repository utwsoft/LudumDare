using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curtain : MonoBehaviour {

    public bool IsMoving = false;
    
    public float Rate = 3.0f;

    public float StopDistance = 5.0f;

    private float StartPosX = 0.0f;

    void Awake()
    {
        
    }

	// Use this for initialization
	void Start () {
        StartPosX = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
	
        if (IsMoving)
        {
            Vector3 pos = transform.position;
            pos.x += Rate * Time.deltaTime;

            transform.position = pos;

            float delta = Mathf.Abs(pos.x - StartPosX);

            if (delta > StopDistance)
            {
                IsMoving = false;
            }
        }	
	}
}
