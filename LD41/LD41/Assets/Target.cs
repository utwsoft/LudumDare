using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    private float Timer = 0.0f;

    public float Speed = 0.50f;

    public bool Flipped = false;

    public bool mKnockedDown = false;

    private float mKnockdownAngle = 0.0f;

    public int PointValue = 100;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Timer += Time.deltaTime;

        Vector3 pos = transform.position;
        pos.x += Speed;
        transform.position = pos;

        Quaternion q1 = Quaternion.AngleAxis(Flipped ? 180.0f : 0.0f, Vector3.up);
        Quaternion q2 = Quaternion.identity;

        if (mKnockedDown && mKnockdownAngle < 90.0f)
        {
            mKnockdownAngle += Time.deltaTime * 400.0f;
        }

        q2 = Quaternion.AngleAxis(mKnockdownAngle, Vector3.right);
        

        transform.rotation = q2 * q1;
    }


    public void KnockDown()
    {
        mKnockedDown = true;
    }
}
