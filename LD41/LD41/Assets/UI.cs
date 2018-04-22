using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public Text Ammo;
    public Text Score;
    public Text Health;

    public GameObject HitIndicatorPanel;
    public GameObject GameOverPanel;

    private bool AmmoLarge = false;
    private bool HealthLarge = false;

    private float curAmmoLargeTime = 0.0f;
    private float curHealthLargeTime = 0.0f;

    public float AmmoLargeTime = 1.0f;
    public float HealthLargeTime = 1.0f;

    private static float kLargeScale = 2.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (HealthLarge)
        {
            curHealthLargeTime += Time.deltaTime;
            if (curHealthLargeTime >= HealthLargeTime)
            {
                RectTransform rect = Health.GetComponent<RectTransform>();
                rect.localScale = Vector3.one;
                HealthLarge = false;
            }
        }

        if (AmmoLarge)
        {
            curAmmoLargeTime += Time.deltaTime;
            if (curAmmoLargeTime >= AmmoLargeTime)
            {
                RectTransform rect = Ammo.GetComponent<RectTransform>();
                rect.localScale = Vector3.one;
                AmmoLarge = false;
            }
        }
    }

    public void MakeHealthBig()
    {
        HealthLarge = true;

        RectTransform rect = Health.GetComponent<RectTransform>();
        rect.localScale = new Vector3(kLargeScale, kLargeScale, kLargeScale);

        curHealthLargeTime = 0.0f;
    }

    public void MakeAmmoBig()
    {
        AmmoLarge = true;

        RectTransform rect = Ammo.GetComponent<RectTransform>();
        rect.localScale = new Vector3(kLargeScale, kLargeScale, kLargeScale);

        curAmmoLargeTime = 0.0f;
    }
}
