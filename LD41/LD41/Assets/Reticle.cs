using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reticle : MonoBehaviour {

    public Image ReticleImage;

    public Canvas UICanvas;

	// Use this for initialization
	void Start () {
		
	}

    void Test()
    {

    }
	
	// Update is called once per frame
	void Update () {
        Vector2 pos;

        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UICanvas.transform as RectTransform, Input.mousePosition, UICanvas.worldCamera, out pos);
        ReticleImage.transform.position = UICanvas.transform.TransformPoint(pos);
    }
}
