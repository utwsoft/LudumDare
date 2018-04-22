using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    public enum Kind
    {
        Duck,
        Badguy,
        Nun
    }

    public Kind TargetKind;

    private float Timer = 0.0f;

    public float Speed = 0.50f;

    public bool Flipped = false;

    public bool mKnockedDown = false;

    private float mKnockdownAngle = 0.0f;

    public float PopupTime = 2.0f;

    public int PointValue = 100;

    private AudioSource Sound;

    public AudioClip TargetHit;

    public AudioClip TargetDuckHit;

    public AudioClip TargetNunHit;

    // Use this for initialization
    void Start () {
        Sound = GetComponent<AudioSource>();
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

        Sound.PlayOneShot(TargetHit);

        if (TargetKind == Kind.Duck)
        {
            Sound.PlayOneShot(TargetDuckHit);
        }
        else if (TargetKind == Kind.Nun)
        {
            Sound.PlayOneShot(TargetNunHit);
        }
    }

    public void PopUp()
    {

    }

    public void StartDown()
    {
        mKnockdownAngle = 89.0f;
    }


    public void SetKind(Kind kind)
    {
        TargetKind = kind;

        Badguy badguy = GetComponent<Badguy>();
        Transform badXfrm = transform.Find("badguy");
        Transform duckXfrm = transform.Find("duck");
        Transform nunXfrm = transform.Find("nun");


        badguy.enabled = kind == Kind.Badguy;

        if (badXfrm != null)
        {
            badXfrm.gameObject.SetActive(kind == Kind.Badguy);
        }

        if (duckXfrm != null)
        {
            duckXfrm.gameObject.SetActive(kind == Kind.Duck);
        }

        if (nunXfrm != null)
        {
            nunXfrm.gameObject.SetActive(kind == Kind.Nun);
        }
    }
}
