using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderWarmUp : MonoBehaviour
{	
	void Start ()
    {

        Shader.WarmupAllShaders();
    }
	
}
