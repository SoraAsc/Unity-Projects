using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    GameManager gameManager;
    public int lineNum;
    public int colNum;
    public GameObject holderCardPrefab;
    [SerializeField] List<GameConstant.AllCardTypes> cardTypes;

    List<Card> allCards;
    List<GameConstant.CardInfo> allCardsWInfo;
    GameConstant.CardGame[,] cardsInGame = new GameConstant.CardGame[6, 6];


    public bool wait;

    GameConstant.CardGame lastGameCard;

    #region Extra
    int failed;
    int maxFailed;
    int currentScore;
    public int comboCount;
    #endregion

    #region GUI
    public Text failedText;
    public Text scoreText;
    public Text comboText;
    #endregion
    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        (lineNum, colNum, cardTypes) = gameManager.GetValues();

        if (lineNum > 0 && colNum > 0) cardsInGame = new GameConstant.CardGame[lineNum, colNum];

        int num = lineNum > colNum ? lineNum : colNum;
        failed = 0;
        maxFailed = 5*num;
        comboCount = 0;
        currentScore = 0;
        wait = false;
        transform.GetChild(0).GetComponent<GridLayoutGroup>().cellSize = new Vector2(CardGridSize(num), CardGridSize(num));
        transform.GetChild(0).GetComponent<GridLayoutGroup>().constraintCount = colNum;

        PickCards();
        RefreshScore();

    }

    public bool CheckTheResult()
    {
        bool won=true;
        for(int i = 0; i < lineNum; i++)
        {
            for(int j = 0; j < colNum; j++)
            {
                if(!cardsInGame[i,j].behaviour.locked) won = false;
            }
        }
        return won;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("sceneTheme");
    }

    public void Restart()
    {
        SceneManager.LoadScene("scene001");
    }

    public void VerifyMatch(int linePos, int colPos)
    {
        cardsInGame[linePos, colPos].behaviour.locked = true;
        if (lastGameCard != null&& comboCount%2==0)
        {
            if (lastGameCard.card.id == cardsInGame[linePos, colPos].card.id && lastGameCard.card.cardType == cardsInGame[linePos, colPos].card.cardType)
            {
                //Debug.Log("Same");
                if (CheckTheResult()) BackToMenu();
                lastGameCard = null;
            }
            else
            {
                //Debug.Log("Not the same");
                cardsInGame[linePos, colPos].behaviour.locked = false;
                lastGameCard.behaviour.locked = false;
                cardsInGame[linePos, colPos].behaviour.TurnInvisible();
                lastGameCard.behaviour.TurnInvisible();
                comboCount = 0;
                failed++;
                if (failed >= maxFailed) Restart();

            }
        }
        lastGameCard = cardsInGame[linePos, colPos];

        RefreshScore();
    }
    
    public void RefreshScore()
    {
        comboText.text = "Combo:\n" + comboCount;
        currentScore += 2 + comboCount * 2;
        scoreText.text = "Score:\n" + currentScore;
        failedText.text = "Failed: " + failed + "/" + maxFailed;
    }

    public void CreateCards(GameConstant.CardGame cardGame,int i, int j)
    {
        GameObject newCard = Instantiate(holderCardPrefab);
        newCard.transform.GetChild(0).GetComponent<Image>().sprite = cardGame.card.artwork;
        newCard.transform.SetParent(transform.GetChild(0));
        newCard.transform.localScale = Vector3.one;
        newCard.GetComponent<CardBehaviour>().SetValues(i, j);
        cardsInGame[i, j].behaviour = newCard.GetComponent<CardBehaviour>();
    }

    public void PickCards()
    {
        allCards = new List<Card>();
        allCardsWInfo = new List<GameConstant.CardInfo>();

        for (int i = 0; i < cardTypes.Count; i++)
        {
            switch (cardTypes[i])
            {
                case GameConstant.AllCardTypes.Fruit:
                    allCards.AddRange(Resources.LoadAll<Card>("Theme/Fruit"));
                    break;
                default:
                    allCards.AddRange(Resources.LoadAll<Card>("Theme/Number"));
                    break;
            }
        }

        int maxCombination = 2;
        for(int i = 0; i < (lineNum * colNum) / maxCombination; i++)
        {
            allCardsWInfo.Add(
                new GameConstant.CardInfo 
                {
                    card = Instantiate(allCards[Random.Range(0, allCards.Count)]),                   
                    max = maxCombination
                });
        }

        for (int i = 0; i < lineNum; i++)
        {
            for (int j = 0; j < colNum; j++)
            {
                List<GameConstant.CardInfo> allCardsWInfoTemp = allCardsWInfo.FindAll(item => item.max > 0);
                GameConstant.CardInfo cardInfo = allCardsWInfoTemp[Random.Range(0, allCardsWInfoTemp.Count)];
                cardInfo.max--;
                cardsInGame[i, j] = new GameConstant.CardGame { card = cardInfo.card };
                CreateCards(cardsInGame[i, j], i, j);
            }
        }
    }
    public int CardGridSize(int num)
    {
        return (800 / num) + 12;
    }
}
