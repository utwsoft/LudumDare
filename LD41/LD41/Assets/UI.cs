using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UI : MonoBehaviour {

    public Text Ammo;
    public Text Score;
    public Text Health;

    public Text AmmoBonus;
    public Text HealthBonus;

    private int DisplayAmmoValue;
    private int TargetAmmoValue;

    private int DisplayHealthValue;
    private int TargetHealthValue;

    private int DisplayScore;
    private int TargetScore;

    public GameObject HitIndicatorPanel;
    public GameObject GameOverPanel;

    private static float kLargeScale = 2.0f;

    private float mCounter = 0.0f;

    private static float kCountTickerDelay = 0.01f;


    private float mWaveCounter = 0.0f;

    private float AmmoBonusCounter = 0.0f;
    public float AmmoBonusDisplayTime = 1.0f;

    private float HealthBonusCounter = 0.0f;
    public float HealthBonusDisplayTime = 1.0f;

    // Use this for initialization
    void Start () {
        ResetBonuses();   
    }
    
    // Update is called once per frame
    void Update () {

        mWaveCounter += Time.deltaTime;

        mCounter += Time.deltaTime;


        if (mCounter >= kCountTickerDelay)
        {
            mCounter = 0.0f;

            if (DisplayAmmoValue != TargetAmmoValue)
            {
                int dir = TargetAmmoValue - DisplayAmmoValue;

                DisplayAmmoValue += (dir > 0) ? 1 : -1;
            }

            if (DisplayHealthValue != TargetHealthValue)
            {
                int dir = TargetHealthValue - DisplayHealthValue;

                DisplayHealthValue += (dir > 0) ? 1 : -1;
            }

            if (DisplayScore != TargetScore)
            {
                int dir = TargetScore - DisplayScore;

                DisplayScore += (dir > 0) ? 10 : -10;

                if (dir > 0)
                {
                    if (DisplayScore > TargetScore)
                        DisplayScore = TargetScore;
                }
                else if (dir < 0)
                {
                    if (DisplayScore < TargetScore)
                        DisplayScore = TargetScore;
                }
            }
        }
        
        Ammo.text = DisplayAmmoValue.ToString();
        Health.text = DisplayHealthValue.ToString();
        Score.text = DisplayScore.ToString();


        if (AmmoBonus.gameObject.activeSelf)
        {
            AmmoBonusCounter += Time.deltaTime;
            if (AmmoBonusCounter > AmmoBonusDisplayTime)
            {
                AmmoBonusCounter = 0.0f;
                AmmoBonus.gameObject.SetActive(false);
            }
        }

        if (HealthBonus.gameObject.activeSelf)
        {
            HealthBonusCounter += Time.deltaTime;
            if (HealthBonusCounter > HealthBonusDisplayTime)
            {
                HealthBonusCounter = 0.0f;
                HealthBonus.gameObject.SetActive(false);
            }
        }

        PulsateRect(AmmoBonus.GetComponent<RectTransform>(), mWaveCounter);
        PulsateRect(HealthBonus.GetComponent<RectTransform>(), mWaveCounter);
    }

    public void SetTargetAmmo(int ammo, bool immediate)
    {
        TargetAmmoValue = ammo;
        if (immediate)
        {
            DisplayAmmoValue = ammo;
        }
    }

    public void SetTargetHealth(int health, bool immediate)
    {
        TargetHealthValue = health;
        if (immediate)
        {
            DisplayHealthValue = health;
        }
    }

    public void SetTargetScore(int score, bool immediate)
    {
        TargetScore = score;
        if (immediate)
        {
            DisplayScore = score;
        }
    }

    private void PulsateRect(RectTransform rect, float time)
    {
        float rate = 10.0f;
        float baseScale = 1.6f;

        float swing = 0.1f;

        float sin = Mathf.Sin(time * rate);

        float scale = baseScale + swing * sin;
        rect.localScale = new Vector3(scale, scale, scale);
    }

    public void ShowAmmoBonus(int bonus)
    {
        AmmoBonus.text = "+ " + bonus.ToString();
        AmmoBonusCounter = 0.0f;

        AmmoBonus.gameObject.SetActive(true);
    }

    public void ShowHealthBonus(int bonus)
    {
        string str = (bonus > 0) ? "+ " : "- "; 
        HealthBonus.text = str + Mathf.Abs(bonus).ToString();
        HealthBonus.color = (bonus > 0) ? Color.green : Color.red;
        HealthBonusCounter = 0.0f;

        HealthBonus.gameObject.SetActive(true);
    }

    public void ResetBonuses()
    {
        AmmoBonus.gameObject.SetActive(false);
        HealthBonus.gameObject.SetActive(false);
    }
}
