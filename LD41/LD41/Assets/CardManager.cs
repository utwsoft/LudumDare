using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public List<GameObject> Cards = new List<GameObject>();

    public Texture2D Question;
    public Texture2D Ammo;
    public Texture2D Health;
    public Texture2D Poison;
    public Texture2D Poison2;
    public Texture2D Block;
    
    public Card FirstCard = null;
    public Card SecondCard = null;

    private bool isEvaluating = false;

    private float evaluationTimer = 0.0f;
    private static float kMaxEvaluationTime = 0.7f;

    private bool isMatchFound = false;

    public delegate void CardMatchHandler(Card.CardValue value);

    public CardMatchHandler cardMatchHandler = null;

    private void Awake()
    {
        Transform firstCard = transform.GetChild(0);

        for (int i = 1; i < 9; ++i)
        {
            GameObject obj = GameObject.Instantiate(firstCard.gameObject, transform) as GameObject;
            obj.name = "card" + i.ToString();

            Vector3 stepRight = new Vector3(0.25f, 0.0f, 0.0f);
            Vector3 stepDown = new Vector3(0.0f, -0.25f, 0.0f);


            int col = i / 3;
            int row = i % 3;

            Vector3 pos = obj.transform.localPosition;
            pos.x += (float)col * stepRight.x;
            pos.y += (float)row * stepDown.y;

            obj.transform.localPosition = pos;
        }
        Cards.Clear();
        foreach (Transform child in transform)
        {
            Cards.Add(child.gameObject);
        }

        ResetAll(true);
    }
    // Use this for initialization
    void Start () {
    }

    void Update ()
    {
        if (isEvaluating)
        {
            evaluationTimer += Time.deltaTime;
            if (evaluationTimer > kMaxEvaluationTime)
            {
                evaluationTimer = 0.0f;
                ResetAll(isMatchFound);
                isEvaluating = false;
            }
        }
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
                    if (card != null && !card.Revealed && !isEvaluating)
                    {
                        if (FirstCard == null && SecondCard == null)
                        {

                            FirstCard = card;
                            SetTexture(obj, GetTextureForCardValue(card.Value));
                            card.Revealed = true;


                        }
                        else if (FirstCard != null && SecondCard == null)
                        {

                            SecondCard = card;
                                SetTexture(obj, GetTextureForCardValue(card.Value));
                            card.Revealed = true;


                            Evaluate();
                        }
                    }
                }
            }
        }

    }

    private void Evaluate()
    {
        isEvaluating = true;
        // Start a reveal timer. for evaluation

        isMatchFound = FirstCard.Value == SecondCard.Value;
        FirstCard.ShowResult(isMatchFound);
        SecondCard.ShowResult(isMatchFound);

        if (isMatchFound && cardMatchHandler != null)
            cardMatchHandler(FirstCard.Value);
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
            case Card.CardValue.Block:
                return Block;
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

            ++index;
        }
    }

    private void HideAllCards()
    {
        int index = 0;
        foreach (GameObject c in Cards)
        {
            Card card = c.GetComponent<Card>();
            card.Revealed = false;

            ++index;
        }
    }

    private void ResetAll(bool newCardValues)
    {
        ResetImages();
        HideAllCards();

        if (newCardValues)
            InitCardValues();



        if (FirstCard != null)
            FirstCard.HideResult();

        if (SecondCard != null)
            SecondCard.HideResult();

        FirstCard = null;
        SecondCard = null;
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
