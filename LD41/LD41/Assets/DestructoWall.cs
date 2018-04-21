using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructoWall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject)
        {
            Target tgt = collision.gameObject.GetComponent<Target>();
            if (tgt != null)
            {
                GameObject.Destroy(tgt.gameObject);
            }
        }
    }
}
