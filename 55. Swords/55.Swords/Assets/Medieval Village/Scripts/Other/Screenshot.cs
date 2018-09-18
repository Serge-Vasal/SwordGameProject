using UnityEngine;
using System.Collections;

public class Screenshot : MonoBehaviour 
{

    public int SizeScreen = 1;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ScreenCapture.CaptureScreenshot("Screenshot + " + Time.time + ".png", SizeScreen);
        }
	}
}
