using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    public enum CardValue
    {
        Unknown,
        Ammo,
        Health,
        Poison,
        Poison2,
        Health100,
        Block
    }

    public CardValue Value;

    public bool Revealed = false;

    public GameObject Result;

    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        
    }

    public void SetResult(Color color, float size)
    {
        if (Result == null)
            return;

        Renderer r = Result.GetComponent<Renderer>();
        r.material.color = color;

        Result.transform.localScale = new Vector3(size, 1.0f, size);

    }
    public void ShowResult(bool isMatch)
    {
        SetResult(isMatch? Color.green: Color.red, 1.25f);
    }



    public void HideResult()
    {
        SetResult(Color.black, 1.0f);
    }
}
