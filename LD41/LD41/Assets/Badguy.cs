using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Badguy : MonoBehaviour {

    public GameObject Muzzle1;
    public GameObject Muzzle2;

    public GameObject PrefabMuzzleFlash;

    public float curTime = 0.0f;
    public float curFlashTime = 0.0f;

    public float FireTime = 3.0f;

    private Target target = null;

    private static float kFlashLength = 0.5f;   // just a small fraction of a second.

    public bool flash = false;

    void Awake()
    {
        target = transform.GetComponent<Target>();

        //Muzzle1 = GameObject.Find("muzzle1");
        //Muzzle2 = GameObject.Find("muzzle2");

        //SetMuzzleFlashActive(false);

        flash = false;
    }
    // Use this for initialization
    void Start () {

        flash = false;

        StartCoroutine("DoTurnOffMuzzleFlash");
    }

    IEnumerator DoTurnOffMuzzleFlash()
    {
        yield return new WaitForEndOfFrame();

        //SetMuzzleFlashActive(false);
    }
    
    // Update is called once per frame
    void Update () {

        if (target.mKnockedDown)
            return;

        if (!flash)
        {
            curTime += Time.deltaTime;

            if (curTime > FireTime)
            {
                curTime = 0.0f;
                curFlashTime = 0.0f;

                //Vector2 pos = transform.localPosition;
                //pos.y += 0.5f;
                //transform.localPosition = pos;

                InstantiateMuzzleFlash();

                //SetMuzzleFlashActive(true);

                SendGameMessageOfAttack();

                flash = true;
            }
        }
        else
        {
            curFlashTime += Time.deltaTime;

            if (curFlashTime > kFlashLength)
            {
                //Vector2 pos = transform.localPosition;
                //pos.y -= 0.5f;
                //transform.localPosition = pos;

                curTime = 0.0f;
                curFlashTime = 0.0f;

                //SetMuzzleFlashActive(false);

                flash = false;
            }
        }
    }

    //void SetMuzzleFlashActive(bool active)
    //{
    //    if (MuzzleFlash1 != null && MuzzleFlash1.activeSelf != active)
    //        MuzzleFlash1.SetActive(active);

    //    if (MuzzleFlash2 != null && MuzzleFlash2.activeSelf != active)
    //        MuzzleFlash2.SetActive(active);
    //}

    private void InstantiateMuzzleFlash()
    {
        if (PrefabMuzzleFlash != null && Muzzle1 != null && Muzzle2 != null)
        {
            GameObject.Instantiate(PrefabMuzzleFlash, Muzzle1.transform);
            GameObject.Instantiate(PrefabMuzzleFlash, Muzzle2.transform);
        }
    }

    void SendGameMessageOfAttack()
    {
        // This is the worst part of a bad design. Don't do this, kids! Use a messaging system instead!
        Main.Get().TakeDamage();
    }
}
