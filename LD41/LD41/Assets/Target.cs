using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    private float Timer = 0.0f;

    public float Speed = 0.50f;

    private bool mKnockedDown = false;

    private float mKnockdownAngle = 0.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Timer += Time.deltaTime;

        Vector3 pos = transform.position;
        pos.x += Speed;
        transform.position = pos;

        if (mKnockedDown && mKnockdownAngle < 90.0f)
        {
            mKnockdownAngle += Time.deltaTime * 400.0f;

            Quaternion q = Quaternion.AngleAxis(mKnockdownAngle, Vector3.right);
            transform.localRotation = q;
        }
    }


    public void KnockDown()
    {
        mKnockedDown = true;
    }
}
