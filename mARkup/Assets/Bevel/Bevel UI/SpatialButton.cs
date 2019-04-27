using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Bevel;

/*Probably good as a base class for all spatial buttons. Then specific uses could inherit from this? 
 * Or those other things are separate scripts that will also be attached. That might make more sense.
 */

public class SpatialButton : MonoBehaviour {

    //Exposed to the inspector. Load up with whatever you want to happen when this button is clicked. 
    public UnityEvent calledOnClick = new UnityEvent();

    void Start()
    {

        if (calledOnClick == null)
            calledOnClick = new UnityEvent();

        BevelInput.clickButtonEvent += CallClickEvent;
        BevelInput.clickButtonEventCropped += CallClickEvent;
    }

    public void TurnOnOtherButtons(GameObject newButton)
    {
        newButton.SetActive(true);
    }
    public void TurnOffButton(GameObject button)
    {
        button.SetActive(false);
    }
    public void TurnOffSelf()
    {
        gameObject.SetActive(false);
    }

    //calls the calledOnClick unity event. To translate delegate subscription to unity event subscription. Now both are valid.
    //also bypasses the need to for local methods to take the gameObject argument. Because we know it's this one. 
    public void CallClickEvent(GameObject clickedObject)
    {
        if (clickedObject == gameObject)
        {
            calledOnClick.Invoke();
        }
    }

    //used for only debugging. 
    public void Ping()
    {
        Debug.Log("Ping! Spatial button clicked on gameObject: " + gameObject.name);
    }
}
