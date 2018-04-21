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
        Block
    }

    public CardValue Value;

    public bool Revealed = false;

    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        
    }
}
