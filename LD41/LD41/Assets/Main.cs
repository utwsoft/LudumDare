using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public GameObject Player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.name);
        }

        if (Input.GetKey(KeyCode.D))
        {
            Vector3 pos = Player.transform.position;
            pos += new Vector3(0.1f, 0.0f, 0.0f);
            Player.transform.position = pos;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Vector3 pos = Player.transform.position;
            pos -= new Vector3(0.1f, 0.0f, 0.0f);
            Player.transform.position = pos;
        }
    }
}
