using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour {

    public float DecayRate = 2.0f;

    public float Alpha = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Alpha -= DecayRate * Time.deltaTime;

        Renderer r = GetComponent<Renderer>();
        Color color = r.material.color;
        color.a = Alpha;
        r.material.color = color;

        if (Alpha < 0.0f)
        {
            GameObject.Destroy(gameObject);
        }
	}
}
