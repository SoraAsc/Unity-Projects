using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardBehaviour : MonoBehaviour, IPointerDownHandler
{
    int linePos;
    int colPos;
    Image cardImage;
    [SerializeField] float delayToShowUn;
    [SerializeField] float delayToShowBegin;
    [SerializeField] float value;
    bool wait;
    Game game;

    public bool locked;
    private void Start()
    {
        locked = false;
        cardImage = transform.GetChild(0).GetComponent<Image>();
        StartCoroutine(AlphaChange(true,delayToShowBegin));
        game = GetComponentInParent<Game>();
    }

    IEnumerator AlphaChange(bool isShowing, float delay, float signal=-1)
    {
        wait = true;
        float a = cardImage.color.a;
        float tempValue = value*signal;
        if (isShowing)
        {
            while (cardImage.color.a > 0)
            {
                a += tempValue;
                cardImage.color = new Color(1f, 1f, 1f, a);
                yield return new WaitForSeconds(delay * Time.deltaTime);
            }
        }
        else
        {
            while (cardImage.color.a < 1)
            {
                a += tempValue;
                cardImage.color = new Color(1f, 1f, 1f, a);
                yield return new WaitForSeconds(delayToShowUn * Time.deltaTime);
            }
        }
        wait = false;
        game.wait = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!wait && !locked&&!game.wait)
        {
            game.wait = true;
            StartCoroutine(Match());
        }
    }
    IEnumerator Match()
    {
        StartCoroutine(AlphaChange(false,delayToShowUn, 1));

        while (wait) yield return null;
        game.comboCount++;

        //verify if as the same
        game.VerifyMatch(linePos, colPos);
    }

    public void TurnInvisible()
    {
        StartCoroutine(AlphaChange(true,delayToShowUn));
    }

    public void SetValues(int newLinePos, int newColPos)
    {
        linePos = newLinePos;
        colPos = newColPos;
    }

}
