using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBar : MonoBehaviour
{
    //Default value of the slider bar components.     
    //readonly float baseHeightSlider = 6.6875f;
    //readonly float topPosY = -17.8f;


    [Header("Slider Attributes")]
    [SerializeField]
    RectTransform sliderBarHolder, topSliderBar;
    [SerializeField]
    Slider slider;
    readonly float delayToChange = 0.2f, multiply = 6.6875f;


    int maxValue = 32;

    //public int MaxValue { get => maxValue; set => maxValue = value; }

    public void RunMaxCapacity(int newMaxValue)
    {
        maxValue = newMaxValue;
        StartCoroutine(IncreaseSliderMaxCapacity(maxValue));
    }

    public void RunLifeChange(int runs = 1, int signal = 1)
    {
        StartCoroutine(ChangeSliderValue(runs, delayToChange, signal));
    }

    IEnumerator IncreaseSliderMaxCapacity(int runs = 1, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        if (runs > 0 && slider.maxValue < maxValue)
        {
            runs--;
            SliderSizeIncrease();
            slider.maxValue += 1;
            slider.value += 1;
            StartCoroutine(IncreaseSliderMaxCapacity(runs, delay));
        }
    }

    IEnumerator ChangeSliderValue(int runs = 1, float delay = 0, int signal = 1)
    {
        yield return new WaitForSeconds(delay);
        if (runs > 0 && slider.value <= slider.maxValue)
        {
            runs--;
            slider.value += signal;
            StartCoroutine(ChangeSliderValue(runs, delay, signal));
        }
    }

    private void SliderSizeIncrease()
    {
        sliderBarHolder.anchoredPosition += new Vector2(0, -multiply);
        topSliderBar.anchoredPosition += new Vector2(0, multiply);
        slider.GetComponent<RectTransform>().sizeDelta += new Vector2(0, multiply);
    }
}
