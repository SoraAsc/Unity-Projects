using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSelect : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private float yPos;

    private Color32[] selectedColors;


    private void Start()
    {
        selectedColors = new Color32[2];
        selectedColors[0] = new Color32(152, 182, 219, 255);
        selectedColors[1] = new Color32(238, 198, 18, 255); //selected
        if (name.Equals("ButtonStart"))
        {
            EventSystem ev = EventSystem.current;
            ev.SetSelectedGameObject(gameObject);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        eventData.selectedObject.transform.GetChild(0).GetComponent<Text>().color = selectedColors[1];
        player.position = new Vector2(player.position.x,yPos);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        eventData.selectedObject.transform.GetChild(0).GetComponent<Text>().color = selectedColors[0];        
    }


}

