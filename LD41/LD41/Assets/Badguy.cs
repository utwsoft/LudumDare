using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Badguy : MonoBehaviour {

    GameObject MuzzleFlash1;
    GameObject MuzzleFlash2;

    public float curTime = 0.0f;
    public float curFlashTime = 0.0f;

    public float FireTime = 3.0f;

    private static float kFlashLength = 0.1f;   // just a small fraction of a second.

    public bool flash = false;

    void Awake()
    {
        MuzzleFlash1 = GameObject.Find("muzzleflash1");
        MuzzleFlash2 = GameObject.Find("muzzleflash2");

        SetMuzzleFlashActive(false);
    }
    // Use this for initialization
    void Start () {

        flash = false;

        StartCoroutine("DoTurnOffMuzzleFlash");
    }

    IEnumerator DoTurnOffMuzzleFlash()
    {
        yield return new WaitForSeconds(0.5f);

        SetMuzzleFlashActive(false);
    }
    
    // Update is called once per frame
    void Update () {

        if (!flash)
        {
            curTime += Time.deltaTime;

            if (curTime > FireTime)
            {
                curTime = 0.0f;
                curFlashTime = 0.0f;

                flash = true;

                SetMuzzleFlashActive(true);
            }
        }
        else
        {
            curFlashTime += Time.deltaTime;

            if (curFlashTime > kFlashLength)
            {
                curTime = 0.0f;
                curFlashTime = 0.0f;

                SetMuzzleFlashActive(false);

                flash = false;
            }
        }
    }

    void SetMuzzleFlashActive(bool active)
    {
        if (MuzzleFlash1 != null && MuzzleFlash1.activeSelf != active)
            MuzzleFlash1.SetActive(active);

        if (MuzzleFlash2 != null && MuzzleFlash2.activeSelf != active)
            MuzzleFlash2.SetActive(active);
    }
}
