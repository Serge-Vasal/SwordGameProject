using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Slider loadingSlider;

    public void UpdateLoadingSlider(float progressValue)
    {
        loadingSlider.value = progressValue;
    }
}
