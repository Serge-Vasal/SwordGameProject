using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedScrollingText : MonoBehaviour, IAttackable
{
    public Color TextColor;
    public GameObject scrollingTextPrefab;

    private GameObject scrollingTextObject;
    private ScrollingText scrollingTextScript;

    private void Start()
    {
        scrollingTextObject=Instantiate(scrollingTextPrefab, transform.position, Quaternion.identity);
        scrollingTextScript = scrollingTextObject.GetComponent<ScrollingText>();
        scrollingTextObject.SetActive(false);
    }

    public void OnAttack(GameObject attacker, Attack attack)
    {        
        var text = attack.Damage.ToString();
        scrollingTextObject.transform.position = transform.position;
        
        scrollingTextScript.startTime = Time.time;
        scrollingTextScript.SetText(text);
        scrollingTextScript.SetColor(TextColor);
        scrollingTextObject.SetActive(true);
    }
}
