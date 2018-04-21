using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public enum SelectionState
    {
        None,
        First,
        Last
    }


    public List<GameObject> Cards = new List<GameObject>();

    public Texture2D Question;
    public Texture2D Ammo;
    public Texture2D Health;

    public SelectionState State;

    private void Awake()
    {
        Cards.Clear();
        foreach (Transform child in transform)
        {
            Cards.Add(child.gameObject);
        }   
    }
    // Use this for initialization
    void Start () {
    }

    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                GameObject obj = Cards.Find(
                    
                    c => c.name == hit.collider.name
                );
                if (obj != null)
                {
                    Debug.Log("hit: " + obj.name);

                    switch (State)
                    {
                        case SelectionState.None:
                            SetTexture(obj, Ammo);
                            State = SelectionState.First;
                            break;
                        case SelectionState.First:
                            SetTexture(obj, Health);
                            State = SelectionState.Last;
                            break;
                        case SelectionState.Last:
                            ResetAll();
                            State = SelectionState.None;
                            break;
                    }
                }
            }
        }

    }

    private void ResetAll()
    {
        foreach (GameObject obj in Cards)
        {
            SetTexture(obj, Question);
        }
    }

    private void SetTexture(GameObject obj, Texture2D tex)
    {
        Renderer r = obj.GetComponent<Renderer>();
        if (r != null)
        {
            r.material.SetTexture("_MainTex", tex);
        }
    }
}
