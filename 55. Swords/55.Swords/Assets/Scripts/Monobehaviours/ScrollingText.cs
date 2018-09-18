using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingText : MonoBehaviour
{
    public float Duration = 1f;
    public float Speed;
    public float startTime;

    private TextMesh textMesh;    

    private void Awake()
    {
        textMesh = GetComponent<TextMesh>();        
    }

    private void Update()
    {
        if (Time.time - startTime < Duration)
        {            
            transform.LookAt(Camera.main.transform);
            transform.Translate(Vector3.up * Speed * Time.deltaTime);
        }
        else
        {            
            gameObject.SetActive(false);
        }
    }

    public void SetText(string text)
    {
        textMesh.text = text;
    }

    public void SetColor(Color color)
    {
        textMesh.color = color;
    }

}
