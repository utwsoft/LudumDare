using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointManager : MonoBehaviour {

    public GameObject PrefabPointDisplay;

    private List<GameObject> PointBlocks = new List<GameObject>();
    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    public void ShowPoints(GameObject obj, int pts)
    {
        Vector3 pt = obj.transform.position;

        GameObject display = GameObject.Instantiate(PrefabPointDisplay);

        display.transform.parent = transform;

        Vector3 screenPt = Camera.current.WorldToScreenPoint(pt);

        display.transform.position = screenPt;

        Text txt = display.GetComponent<Text>();
        if (txt != null)
        {
            txt.text = (pts > 0) ? ("+" + pts.ToString()) : (pts.ToString());
        }
    }
}
