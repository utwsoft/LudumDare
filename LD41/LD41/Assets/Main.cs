﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public GameObject Player;

    public UI gameUI;
    public CardManager cardManager;

    public GameObject CurtainLeft;
    public GameObject CurtainRight;

    public GameObject Spawners;

    public GameObject Targets;

    public int AmmoCount = 10;
    public int Score = 0;
    public int Health = 100;

    public int BadguyAttackDamage = 3;

    private bool isGameOver = false;

    public static float kStageBounds = 18.0f;

    // Use this for initialization
    void Start () {

        isGameOver = false;

        gameUI.GameOverPanel.SetActive(false);

        UpdateAmmoUI();

        cardManager.cardMatchHandler = OnCardMatch;
    }

    private void UpdateUI()
    {
        UpdateScoreUI();
        UpdateAmmoUI();
        UpdateHealthUI();
    }

    private void UpdateScoreUI()
    {
        gameUI.Score.text = Score.ToString();
    }

    private void UpdateAmmoUI()
    {
        gameUI.Ammo.text = AmmoCount.ToString();
    }

    private void UpdateHealthUI()
    {
        gameUI.Health.text = Health.ToString();
    }

    private void OnCardMatch(Card.CardValue match)
    {
        if (match == Card.CardValue.Ammo)
        {
            AmmoCount += 10;
        }
        else if (match == Card.CardValue.Health)
        {
            Health += 25;
            
        }
        else if (match == Card.CardValue.Poison)
        {
            Health -= 8;
        }

        UpdateUI();
    }
    
    // Update is called once per frame
    void Update () {
        //RaycastHit hit;

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //if (Physics.Raycast(ray, out hit))
        //{
        //    Debug.Log(hit.collider.name);
        //}

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

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                //if (!hit.collider.GetComponentInParent<Card>())
                if (hit.collider.gameObject.tag == "targetable")
                {
                    Fire();
                }
            }
                
        }

        CleanupTargets();
    }

    private void Fire()
    {
        if (AmmoCount > 0)
        {
            if (AmmoCount > 0)
                AmmoCount--;

            UpdateAmmoUI();

            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    
                    Target tgt = hit.collider.GetComponentInParent<Target>();

                    if (tgt != null)
                    {
                        int pointValue = tgt.PointValue;
                        Score += pointValue;
                        UpdateUI();

                        tgt.KnockDown();
                    }
                        
                }
            }
        }
    }

    private void CleanupTargets()
    {
        for (int i = Targets.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = Targets.transform.GetChild(i);
            Vector3 pos = child.transform.position;

            if (pos.x <= -kStageBounds || pos.x > kStageBounds)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    private void DestroyAllTargets()
    {
        for (int i = Targets.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = Targets.transform.GetChild(i);

            GameObject.Destroy(child.gameObject);
        }
    }

    public void TakeDamage()
    {
        if (Health > 0)
        {
            HitIndicator();

            Health -= BadguyAttackDamage;

            Health = Mathf.Max(Health, 0);

            UpdateUI();
        }



        if (Health <= 0 && !isGameOver)
        {
            SetGameOver();
        }
    }

    private void HitIndicator()
    {
        GameObject obj = gameUI.HitIndicatorPanel;

        HitIndicator hit = obj.GetComponent<HitIndicator>();

        hit.GetHit();
    }

    private void SetGameOver()
    {
        isGameOver = true;

        // Stop the spawners
        ActivateSpawners(false);

        for (int i = Targets.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = Targets.transform.GetChild(i);
            Target tgt = child.GetComponent<Target>();
            tgt.Speed = 0.0f;
        }

        
        ActivateCurtain(CurtainLeft);
        ActivateCurtain(CurtainRight);

        gameUI.GameOverPanel.SetActive(true);
    }

    private void ActivateCurtain(GameObject obj)
    {
        if (obj != null)
        {
            Curtain curtain = obj.GetComponent<Curtain>();
            curtain.CurtainState = Curtain.State.Closing;
        }
    }

    private void OpenCurtain(GameObject obj)
    {
        if (obj != null)
        {
            Curtain c = obj.GetComponent<Curtain>();
            c.CurtainState = Curtain.State.Opening;
        }
    }

    private void ActivateSpawners(bool run)
    {
        Spawner[] spawners = Spawners.transform.GetComponentsInChildren<Spawner>();
        foreach (Spawner s in spawners)
        {
            s.IsRunning = run;
        }
    }

    public void OnResetGame()
    {
        DestroyAllTargets();

        ActivateSpawners(true);

        Score = 0;
        Health = 100;
        AmmoCount = 20;

        UpdateUI();

        gameUI.GameOverPanel.SetActive(false);

        OpenCurtain(CurtainLeft);
        OpenCurtain(CurtainRight);

        isGameOver = false;
    }

    // Singleton not fully enforced, but whatever.
    private static Main sInstance = null;

    public static Main Get()
    {
        if (sInstance == null)
        {
            GameObject obj = GameObject.Find("_Game");
            if (obj != null)
            {
                sInstance = obj.GetComponent<Main>();
            }
        }

        return sInstance;
    }
}
