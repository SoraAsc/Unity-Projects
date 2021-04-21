using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    //int maxTiles = 144;
    public Text lineText;
    public Text colText;
    public Slider lineSlider;
    public Slider colSlider;
    public List<Toggle> allTogglesCategories; //0 - Fruit | 1 - Number

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        foreach(Toggle toggle in allTogglesCategories)
        {
            toggle.isOn = false;
            toggle.onValueChanged.AddListener((isOn) => { ToggleValue(isOn, toggle.transform.GetChild(0).GetComponent<Image>()); });
        }
        SetLimit();
        RefreshValues();
    }


    /// <summary>
    /// Sets the min and max value of the slider
    /// </summary>
    public void SetLimit()
    {
        //maxTiles = 144;
        lineSlider.maxValue = 12;
        lineSlider.minValue = 1;
        lineSlider.wholeNumbers = true;

        colSlider.maxValue = 12;
        colSlider.minValue = 1;
        colSlider.wholeNumbers = true;
    }

    public void SliderValueChange()
    {
        RefreshValues();
    }

    public void Play()
    {
        if ((lineSlider.value * colSlider.value) % 2 == 0 && allTogglesCategories.Find(item => item.isOn))
        {
            List<GameConstant.AllCardTypes> allCardTypes = new List<GameConstant.AllCardTypes>();
            for(int i = 0; i < allTogglesCategories.Count; i++)
            {
                if (allTogglesCategories[i].isOn)
                {
                    switch (i)
                    {
                        case 1:
                            allCardTypes.Add(GameConstant.AllCardTypes.Number);
                            break;
                        default:
                            allCardTypes.Add(GameConstant.AllCardTypes.Fruit);
                            break;
                    }
                }
            }
            gameManager.SetValues((int)lineSlider.value, (int)colSlider.value,allCardTypes);
            SceneManager.LoadScene("scene001");
        }
        //else Debug.Log("Something is wrong");
    }

    public void RefreshValues()
    {
        lineText.text = "Line: " + lineSlider.value;
        colText.text = "Column: " + colSlider.value;
    }


    void ToggleValue(bool isOn, Image toggleB)
    {
        toggleB.color = isOn ? new Color32(255, 0, 0,255) : new Color32(255, 255, 255,255);
    }


}
