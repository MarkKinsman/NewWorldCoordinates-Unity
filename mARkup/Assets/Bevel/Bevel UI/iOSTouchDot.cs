using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
using UnityEngine.UI;

//put this on an image and place in a screenspace canvas to track touches and clicks
public class iOSTouchDot : MonoBehaviour {

    RectTransform rectTransform;

	// Use this for initialization
	void Start () {
        rectTransform = gameObject.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount == 1)
        {
            
            Vector2 touchPosition = Input.GetTouch(0).position;
            Vector3 touchPoint = new Vector3(touchPosition.x, touchPosition.y, 0f);
            rectTransform.position = touchPoint;
        }
        else if (Input.GetMouseButton(0))
        {
            
            Vector2 mousePosition = Input.mousePosition;
            Vector3 mousePoint = new Vector3(mousePosition.x, mousePosition.y, 0f);
            rectTransform.position = mousePoint;
        }
        else
        {
            rectTransform.position = new Vector3(-100f, -100f, 0f);
        }

	}
}
