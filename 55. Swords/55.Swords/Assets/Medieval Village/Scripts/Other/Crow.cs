using UnityEngine;
using System.Collections;

public class Crow : MonoBehaviour 
{
    private Animator _animator;

    public float BaseHeight = 17.29f;
    public float SpeedAnimation = 0.9f;
    public float H = 0.0f;
    public float Pause = 1.0f;
    private float LastTime = 0.0f;
	// Use this for initialization
	void Start () 
    {
        _animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        _animator.speed = SpeedAnimation;
        if (Time.time > LastTime + Pause)
        {
            H = H + 0.01f;
            if (H > 360)
            {
                H = 0;
            }
            LastTime = Time.time;
        }
        transform.position = new Vector3 (transform.position.x, BaseHeight + 0.1f*Mathf.Sin(H), transform.position.z);
	}
}
