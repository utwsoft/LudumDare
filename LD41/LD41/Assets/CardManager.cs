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
    public Texture2D Poison;
    public Texture2D Poison2;

    public SelectionState State;

    public Card.CardValue FirstValue = Card.CardValue.Unknown;
    public Card.CardValue SecondValue = Card.CardValue.Unknown;

    //private 

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

                    Card card = obj.GetComponent<Card>();
                    if (card != null && !card.Revealed)
                    {
                        if (FirstValue == Card.CardValue.Unknown && SecondValue == Card.CardValue.Unknown)
                        {

                                FirstValue = card.Value;
                                SetTexture(obj, GetTextureForCardValue(card.Value));
                            card.Revealed = true;


                        }
                        else if (FirstValue != Card.CardValue.Unknown && SecondValue == Card.CardValue.Unknown)
                        {

                                SecondValue = card.Value;
                                SetTexture(obj, GetTextureForCardValue(card.Value));
                            card.Revealed = true;


                            // Start a reveal timer. for evaluation
                        }
                        else
                        {
                            ResetAll();
                        }
                    }





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

    private Texture2D GetTextureForCardValue(Card.CardValue cardValue)
    {
        switch(cardValue)
        {
            case Card.CardValue.Unknown:
                return Question;
                break;
            case Card.CardValue.Ammo:
                return Ammo;
            case Card.CardValue.Health:
                return Health;
            case Card.CardValue.Poison:
                return Poison;
            case Card.CardValue.Poison2:
                return Poison2;
            default:
                return Question;
        }
    }

    private void ResetImages()
    {
        foreach (GameObject obj in Cards)
        {
            SetTexture(obj, Question);
        }
    }

    private void InitCardValues()
    {
        //Random rnd = new Random();
        //string[] MyRandomArray = MyArray.OrderBy(x => rnd.Next()).ToArray();

        List<Card.CardValue> values = new List<Card.CardValue>() {
            Card.CardValue.Ammo,
            Card.CardValue.Ammo,
            Card.CardValue.Health,
            Card.CardValue.Health,
            Card.CardValue.Poison,
            Card.CardValue.Poison,
            Card.CardValue.Poison2,
            Card.CardValue.Poison2,
            Card.CardValue.Block,
        };

        List<Card.CardValue> jumbledValues = new List<Card.CardValue>();

        while (values.Count > 0)
        {

            int idx = Random.Range(0, values.Count);
            jumbledValues.Add(values[idx]);

            values.Remove(values[idx]);
        }

        int index = 0;
        foreach (GameObject c in Cards)
        {
            Card card = c.GetComponent<Card>();
            card.Value = jumbledValues[index];
            card.Revealed = false;

            ++index;
        }
    }

    private void ResetAll()
    {
        ResetImages();
        InitCardValues();
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
