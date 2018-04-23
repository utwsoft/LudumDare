using System.Collections;
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

    public AudioClip GunShot;
    public AudioClip EmptyClick;
    public AudioClip Reload;
    public AudioClip HealthPickup;
    public AudioClip Poison;
    public AudioClip NoMatch;

    private bool isGameOver = false;

    public static float kStageBounds = 18.0f;

    // Use this for initialization
    void Start () {

        isGameOver = false;

        gameUI.GameOverPanel.SetActive(false);

        UpdateUI(true);

        cardManager.cardMatchHandler = OnCardMatch;
    }

    private void UpdateUI(bool immediate)
    { 
        gameUI.SetTargetScore(Score, immediate);
        gameUI.SetTargetAmmo(AmmoCount, immediate);
        gameUI.SetTargetHealth(Health, immediate);
    }

    private void OnCardMatch(Card.CardValue match)
    {
        if (match == Card.CardValue.Ammo)
        {
            AmmoCount += 10;

            gameUI.ShowAmmoBonus(10);
        }
        else if (match == Card.CardValue.Health)
        {
            Health += 25;

            gameUI.ShowHealthBonus(25);
        }
        else if (match == Card.CardValue.Poison)
        {
            Health -= 12;

            gameUI.ShowHealthBonus(-12);

            Health = Mathf.Max(Health, 0);
        }
        else if (match == Card.CardValue.Health100)
        {
            int diff = 100 - Health;
            if (diff > 0)
            {
                gameUI.ShowHealthBonus(diff);
            }

            Health = Mathf.Max(Health, 100);
        }

        UpdateUI(false);

        PlayCardResultSound(match);
    }

    private void PlayCardResultSound(Card.CardValue match)
    {
        AudioSource audio = GetComponent<AudioSource>();

        if (match == Card.CardValue.Ammo)
        {
            audio.PlayOneShot(Reload);  
        }
        else if (match == Card.CardValue.Health)
        {
            audio.PlayOneShot(HealthPickup);
        }
        else if (match == Card.CardValue.Poison)
        {
            audio.PlayOneShot(Poison);
        }
        else if (match == Card.CardValue.Health100)
        {
            audio.PlayOneShot(HealthPickup);
        }
        else if (match == Card.CardValue.Unknown)
        {
            // No match
            audio.PlayOneShot(NoMatch);
        }
    }
    
    // Update is called once per frame
    void Update () {
        //RaycastHit hit;

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //if (Physics.Raycast(ray, out hit))
        //{
        //    Debug.Log(hit.collider.name);
        //}

        //if (Input.GetKey(KeyCode.D))
        //{
        //    Vector3 pos = Player.transform.position;
        //    pos += new Vector3(0.1f, 0.0f, 0.0f);
        //    Player.transform.position = pos;
        //}
        //else if (Input.GetKey(KeyCode.A))
        //{
        //    Vector3 pos = Player.transform.position;
        //    pos -= new Vector3(0.1f, 0.0f, 0.0f);
        //    Player.transform.position = pos;
        //}

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                //if (!hit.collider.GetComponentInParent<Card>())
                if (hit.collider.gameObject.tag == "targetable" && !isGameOver)
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

            UpdateUI(false);

            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(GunShot);

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
                        UpdateUI(false);
                        
                        StartCoroutine("KnockdownTarget", tgt);
                    }
                }
            }
        }
        else
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(EmptyClick);
        }
    }

    // Adds delay to the reaction of the target to the gunfire
    IEnumerator KnockdownTarget(Target tgt)
    {
        yield return new WaitForSeconds(0.20f);

        gameUI.PointMgr.ShowPoints(tgt.gameObject, tgt.PointValue);
        tgt.KnockDown();
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

            UpdateUI(false);
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

        cardManager.IsActive = false;

        // Stop the spawners
        ActivateSpawners(false);

        for (int i = Targets.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = Targets.transform.GetChild(i);
            Target tgt = child.GetComponent<Target>();
            tgt.Speed = 0.0f;

            Badguy bad = child.GetComponent<Badguy>();
            if (bad != null)
            {
                bad.IsAttacking = false;
            }
        }

        
        CloseCurtain(CurtainLeft);
        CloseCurtain(CurtainRight);

        gameUI.GameOverPanel.SetActive(true);
    }

    private void CloseCurtain(GameObject obj)
    {
        if (obj != null)
        {
            Curtain curtain = obj.GetComponent<Curtain>();
            curtain.SetState(Curtain.State.Closing);
            //curtain.CurtainState = Curtain.State.Closing;
        }
    }

    private void OpenCurtain(GameObject obj)
    {
        if (obj != null)
        {
            Curtain c = obj.GetComponent<Curtain>();
            //c.CurtainState = Curtain.State.Opening;
            c.SetState(Curtain.State.Opening);
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

        UpdateUI(true);

        gameUI.GameOverPanel.SetActive(false);
        gameUI.ResetBonuses();

        cardManager.IsActive = true;

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
