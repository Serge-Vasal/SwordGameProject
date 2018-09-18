using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider healthSlider;
    private int maxHealth;

    public Color redColor;
    public Color yellowColor;
    public Color greenColor;
    public Image fillImage;

    private void Awake()
    {
        healthSlider = gameObject.GetComponent<Slider>();
    }

    public void SetMaxHealthSliderValue(int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        this.maxHealth = maxHealth;
    }

    public void UpdateHealthSlider(int health)
    {
        healthSlider.value = health;

        if (health >= maxHealth * 0.7f)
        {
            fillImage.color = greenColor;
        }
        else if (health <= maxHealth * 0.7f && health >= maxHealth * 0.3f)
        {
            fillImage.color = yellowColor;
        }
        else
        {
            fillImage.color = redColor;
        }
    }   

}
