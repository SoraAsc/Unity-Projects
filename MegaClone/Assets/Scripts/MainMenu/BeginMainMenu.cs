using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BeginMainMenu : MonoBehaviour
{
    [SerializeField]
    private Animator ani;
    [SerializeField]
    private GameObject logoHolder;


    [SerializeField]
    private Transform playerWeaponArea;
    [SerializeField]
    private Transform buttonsHolder;

    [SerializeField]
    private GameObject ammoPrefab;

    private int numRepeatFirstAnimation = 6;

    public void ChooseStart()
    {
        ChangeButtonInteraction();
        ChooseAnimation();
    }

    public void ChoosePassword()
    {
        ChangeButtonInteraction();
        ChooseAnimation();
    }

    public void ChooseOption()
    {
        ChangeButtonInteraction();
        ChooseAnimation();
    }

    private void Awake()
    {
        ani = GetComponent<Animator>();
        ani.SetInteger("firstAnimationRepeatNum", numRepeatFirstAnimation);
    }

    #pragma warning disable IDE0051 // Remover membros privados não utilizados
    private void OpenAnimationRepeatCount()
    {
        numRepeatFirstAnimation -= 1;
        ani.SetInteger("firstAnimationRepeatNum", numRepeatFirstAnimation);
    }

    private void ChooseAnimation()
    {
        Instantiate(ammoPrefab, playerWeaponArea);
    }

    private void ActivePlayer()
    {
        playerWeaponArea.parent.gameObject.SetActive(true);
    }

    private void ChangeButtonInteraction()
    {
        EventSystem ev = EventSystem.current;
        GameObject button = ev.currentSelectedGameObject;
        foreach (Transform btn in buttonsHolder)
        {
            btn.GetComponent<Button>().interactable = !btn.GetComponent<Button>().interactable;
        }
        button.transform.GetChild(0).GetComponent<Text>().color = new Color32(238, 198, 18, 255);
    }


}
