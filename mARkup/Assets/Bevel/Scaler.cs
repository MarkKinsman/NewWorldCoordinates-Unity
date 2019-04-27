using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*Yeah this is a totally lazy script. But I think I have a better one 
 * somewhere and I didn't want to rewrite a good one just for this demo.
*/

public class Scaler : MonoBehaviour {

    public enum ScaleName {full, threeEighths };
    public ScaleName scaleName;
    public Text scaleText;

	// Use this for initialization
	void Start () {
        scaleName = ScaleName.full;
        ScaleSwap();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ScaleSwap(){
        if (scaleName == ScaleName.full)
        {
            scaleName = ScaleName.threeEighths;
            SetScale(0.03125f);
            scaleText.text = "3/8\" = 1'";
        }
        else if (scaleName == ScaleName.threeEighths)
        {
            scaleName = ScaleName.full;
            SetScale(1f);
            scaleText.text = "1:1 Full Scale";
        }
    }

    public void SetScale(float factor){
        transform.localScale = new Vector3(factor, factor, factor);
    }

}
