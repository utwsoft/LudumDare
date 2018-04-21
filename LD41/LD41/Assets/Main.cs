using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public GameObject Player;

    public UI gameUI;
    public CardManager cardManager;

    public GameObject Targets;

    public int AmmoCount = 10;
    public int Score = 0;

    // Use this for initialization
    void Start () {
        UpdateAmmoUI();

        cardManager.cardMatchHandler = OnCardMatch;
    }

    private void UpdateUI()
    {
        UpdateScoreUI();
        UpdateAmmoUI();
    }

    private void UpdateScoreUI()
    {
        gameUI.Score.text = Score.ToString();
    }

    private void UpdateAmmoUI()
    {
        gameUI.Ammo.text = AmmoCount.ToString();
    }

    private void OnCardMatch(Card.CardValue match)
    {
        if (match == Card.CardValue.Ammo)
        {
            AmmoCount += 10;
            UpdateAmmoUI();
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

            if (pos.x <= -18.0f || pos.x > 18.0f)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
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
