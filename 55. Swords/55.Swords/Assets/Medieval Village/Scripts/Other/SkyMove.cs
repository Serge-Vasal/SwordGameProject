using UnityEngine;
using System.Collections;

public class SkyMove : MonoBehaviour 
{
    public float smooth = 2.0F;
    public float Start = 2.0F;

    void Update() 
	{
		 transform.Rotate(new Vector3(0,1,0) *smooth* Time.deltaTime, Space.World);
    }
}