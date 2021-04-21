using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    bool isMoving;
    bool isReadjusting;
    bool isFighting;
    bool isDead;
    bool isInBattle;
    bool untilTheEnd; //The fight will occur until the player or the enemy dead.
    Vector3 oriPos, targetPos;
    [SerializeField]
    float timeToMove = 0.2f;
    [SerializeField]
    float duration = 0.1f, waitBetween = 0.2f;
    [SerializeField] 
    float walkRange = 115f;

    [SerializeField]
    Transform allCardHolder;
    public GameObject mainInfo;
    public GameObject contentInfoShow;

    #region Player

    GameObject player;
    private int lv;
    int pityToUpLv = 0;
    public Cards playerMainCard;
    List<Cards> allPlayerCards;
    [SerializeField]
    int linePos = 2, colPos = 2;
    int antLinePos = 2, antColPos = 2;
    #endregion

    
    public List<GameConstant.Tile> tiles;
    [SerializeField]
    List<GameConstant.TilesInZone> tilesInZone;

    public GameObject defaultCardPrefab;

    List<Cards> allCards;
    private void Awake()
    {
        linePos = 2;
        antLinePos = 2;
        colPos = 2;
        antColPos = 2;

        //width + spacing and spacing+height
        allCardHolder = GameObject.FindGameObjectWithTag("AllCardHolder").transform;
        player = GameObject.FindGameObjectWithTag("Player");
        allCards = new List<Cards>();
        allPlayerCards = new List<Cards>();
        tilesInZone = new List<GameConstant.TilesInZone>();

        allCards.AddRange(Resources.LoadAll<Cards>("CardEnemy"));
        allCards.AddRange(Resources.LoadAll<Cards>("CardConsumable"));
        allCards.AddRange(Resources.LoadAll<Cards>("CardEquipment"));
        allPlayerCards.AddRange(Resources.LoadAll<Cards>("CardPlayer"));

        DefaultCardConf(player, playerMainCard);

        tiles.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            tiles.Add(new GameConstant.Tile(transform.GetChild(i).gameObject));
        }
    }

    private void Start()
    {
        playerMainCard = Instantiate(playerMainCard);
        StartCoroutine(AddAllCard());
        isFighting = false;
        isMoving = false;
        isReadjusting = false;
        untilTheEnd = false;
        isDead = false;
        isInBattle = true;
        lv = 1;
        pityToUpLv = 0;
        InfoShow(-1, -1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && !isMoving && player.transform.localPosition.y < 230f && !isReadjusting&&!isFighting&&isInBattle)
        { antLinePos = linePos; antColPos = colPos; linePos--; StartCoroutine(MovePlayer(Vector3.up * walkRange, player.transform)); }
        else if (Input.GetKeyDown(KeyCode.W) && !isMoving && player.transform.localPosition.y < 230f && !isReadjusting && !isFighting && !isInBattle) 
        { InfoShow(linePos - 1, colPos); }

        if (Input.GetKeyDown(KeyCode.S) && !isMoving && player.transform.localPosition.y > -230f && !isReadjusting && !isFighting && isInBattle)
        { antLinePos = linePos; antColPos = colPos; linePos++; StartCoroutine(MovePlayer(Vector3.down * walkRange, player.transform));  }
        else if (Input.GetKeyDown(KeyCode.S) && !isMoving && player.transform.localPosition.y > -230f && !isReadjusting && !isFighting && !isInBattle) 
        { InfoShow(linePos + 1, colPos); }

        if (Input.GetKeyDown(KeyCode.A) && !isMoving && player.transform.localPosition.x > -230f && !isReadjusting && !isFighting && isInBattle)
        { antColPos = colPos; antLinePos = linePos; colPos--; StartCoroutine(MovePlayer(Vector3.left * walkRange, player.transform));  }
        else if (Input.GetKeyDown(KeyCode.A) && !isMoving && player.transform.localPosition.x > -230f && !isReadjusting && !isFighting && !isInBattle)
        { InfoShow(linePos, colPos-1); }

        if (Input.GetKeyDown(KeyCode.D) && !isMoving && player.transform.localPosition.x < 230f && !isReadjusting && !isFighting && isInBattle)
        { antColPos = colPos; antLinePos = linePos; colPos++; StartCoroutine(MovePlayer(Vector3.right * walkRange, player.transform));  }
        else if (Input.GetKeyDown(KeyCode.D) && !isMoving && player.transform.localPosition.x < 230f && !isReadjusting && !isFighting && !isInBattle)
        { InfoShow(linePos, colPos+1); }
    }

    public void DirectionButton(int directionIndex)
    {
        switch( (GameConstant.Directions)directionIndex)
        {
            case GameConstant.Directions.Up:
                if (!isMoving && player.transform.localPosition.y < 230f && !isReadjusting && !isFighting && isInBattle)
                { antLinePos = linePos; antColPos = colPos; linePos--; StartCoroutine(MovePlayer(Vector3.up * walkRange, player.transform));  }
                else if (!isMoving && player.transform.localPosition.y < 230f && !isReadjusting && !isFighting && !isInBattle)
                { InfoShow(linePos - 1, colPos); }
                break;

            case GameConstant.Directions.Down:
                if (!isMoving && player.transform.localPosition.y > -230f && !isReadjusting && !isFighting && isInBattle)
                { antLinePos = linePos; antColPos = colPos; linePos++; StartCoroutine(MovePlayer(Vector3.down * walkRange, player.transform));  }
                else if (!isMoving && player.transform.localPosition.y > -230f && !isReadjusting && !isFighting && !isInBattle)
                { InfoShow(linePos + 1, colPos); }
                break;

            case GameConstant.Directions.Left:
                if (!isMoving && player.transform.localPosition.x > -230f && !isReadjusting && !isFighting && isInBattle)
                { antColPos = colPos; antLinePos = linePos; colPos--; StartCoroutine(MovePlayer(Vector3.left * walkRange, player.transform));  }
                else if (!isMoving && player.transform.localPosition.x > -230f && !isReadjusting && !isFighting && !isInBattle)
                { InfoShow(linePos, colPos - 1); }
                break;

            case GameConstant.Directions.Right:
                if (!isMoving && player.transform.localPosition.x < 230f && !isReadjusting && !isFighting && isInBattle)
                { antColPos = colPos; antLinePos = linePos; colPos++; StartCoroutine(MovePlayer(Vector3.right * walkRange, player.transform));  }
                else if (!isMoving && player.transform.localPosition.x < 230f && !isReadjusting && !isFighting && !isInBattle)
                { InfoShow(linePos, colPos + 1); }
                break;

            default:
                if (!isMoving &&!isReadjusting && !isFighting && !isInBattle) InfoShow(-2, -1);
                break;
        }

    }

    IEnumerator SeekTo(Transform toMove, Vector2 target,float duration)
    {
        isReadjusting = true;

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float time = Vector2.Distance(toMove.localPosition, target) / (duration-elapsedTime) * Time.deltaTime;

            toMove.localPosition = Vector2.MoveTowards(toMove.localPosition, target,time);
            yield return null;
        }
        toMove.localPosition = target;

        isReadjusting = false;
    }

    private IEnumerator MovePlayer(Vector3 direction,Transform toMove)
    {
        isFighting = true;
        StartCoroutine(Fight());
        while (isFighting) { yield return null; }

        if (isDead)
        {
            isDead = false;
            isMoving = true;

            float elapsedTime = 0;

            oriPos = toMove.localPosition;
            targetPos = oriPos + direction;

            while (elapsedTime < timeToMove)
            {
                toMove.localPosition = Vector3.Lerp(oriPos, targetPos,
                    (elapsedTime / timeToMove));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            toMove.localPosition = targetPos;
            isMoving = false;
        }
    }
    IEnumerator Fight()
    {
        GameConstant.TilesInZone cardRemoved = null;
        if (tilesInZone.Count > 0)
            cardRemoved = tilesInZone.Find(item => item.linePos == linePos && item.colPos == colPos);

        if (cardRemoved != null)
        {
            Cards card = cardRemoved.cards;
            pityToUpLv += 1;

            if (pityToUpLv >= 15) { lv += 1; pityToUpLv = 0; }
            switch (card.cardType)
            {
                case GameConstant.AllCardType.Enemy:
                    int critical = 1;
                    do
                    {
                        if (playerMainCard.evasion <= Random.Range(0.000f, 1.000f))
                        {
                            if (card.critRate >= Random.Range(0.000f, 1.000f)) critical = 2;
                            playerMainCard.hp -= Calc.DamageCalc(card, playerMainCard, critical);
                        }
                        if (card.evasion <= Random.Range(0.000f, 1.000f))
                        {
                            if (playerMainCard.critRate >= Random.Range(0.000f, 1.000f)) critical = 2;
                            card.hp -= Calc.DamageCalc(playerMainCard, card, critical);
                        }
                    }  while (untilTheEnd && (card.hp > 0 && playerMainCard.hp > 0));

                    if (card.hp <= 0 && (card.rankOfEnemy != GameConstant.AllRanksOfEnemy.MiniBoss && card.rankOfEnemy != GameConstant.AllRanksOfEnemy.Boss))
                    {
                        isDead = true;
                        RemoveCard(cardRemoved);
                    }
                    else
                    {
                        colPos = antColPos;
                        linePos = antLinePos;
                    }

                    if (card.hp <= 0 && (card.rankOfEnemy == GameConstant.AllRanksOfEnemy.MiniBoss || card.rankOfEnemy == GameConstant.AllRanksOfEnemy.Boss))
                    {
                        GenerateMiniOrBossDrop(card, cardRemoved.linePos, cardRemoved.colPos, cardRemoved.cardObject.transform);
                        RemoveCard(cardRemoved);
                    }
                    break;
                case GameConstant.AllCardType.Equipment:
                    playerMainCard.hp += card.hp;
                    playerMainCard.spPool += card.spPool;
                    playerMainCard.atk += card.atk;
                    playerMainCard.matk += card.matk;
                    playerMainCard.def += card.def;
                    playerMainCard.mdef += card.mdef;
                    playerMainCard.critRate += card.critRate;
                    playerMainCard.evasion += card.evasion;
                    isDead = true;
                    RemoveCard(cardRemoved);
                    break;
                case GameConstant.AllCardType.Consumable:
                    for (int i = 0; i < card.effects.Length; i++)
                    {
                        switch (card.effects[i])
                        {
                            case GameConstant.AllEffects.RecoverHp:
                                if (card.amountEffect > 0)
                                    playerMainCard.hp += card.amountEffect;
                                else playerMainCard.hp += Mathf.FloorToInt(playerMainCard.hp * card.percentageEffect);
                                break;
                            case GameConstant.AllEffects.RecoverMana:
                                if (card.amountEffect > 0)
                                    playerMainCard.spPool += card.amountEffect;
                                else playerMainCard.spPool += Mathf.FloorToInt(playerMainCard.spPool * card.percentageEffect);
                                break;
                            case GameConstant.AllEffects.ChangeAttackType:
                                Cards[] classCard = allPlayerCards.FindAll(item => item.cardClass != playerMainCard.cardClass).ToArray();
                                int classIndex = Random.Range(0, classCard.Length);
                                playerMainCard = GameConstant.CardStatusTransfer(playerMainCard, classCard[classIndex]);
                                DefaultCardConf(player, playerMainCard);
                                break;
                        }
                    }
                    isDead = true;
                    RemoveCard(cardRemoved);
                    break;
            }
            DefaultCardConf(player, playerMainCard);
            DefaultCardConf(cardRemoved.cardObject, cardRemoved.cards);
            if(playerMainCard.hp<=0)
                UnityEngine.SceneManagement.SceneManager.LoadScene("scene000");
            
        }
        isFighting = false;
        yield return null;
    }

    public void RemoveCard(GameConstant.TilesInZone cardRemoved)
    {
        Destroy(cardRemoved.cardObject);
        tilesInZone.Remove(cardRemoved);
        if (cardRemoved.cards.rankOfEnemy != GameConstant.AllRanksOfEnemy.MiniBoss && cardRemoved.cards.rankOfEnemy != GameConstant.AllRanksOfEnemy.Boss)
        {
            if (linePos == antLinePos) StartCoroutine(ReagengeCard(antColPos, colPos, "Col"));
            else StartCoroutine(ReagengeCard(antLinePos, linePos, signal: -1));
        }
        InfoShow(-1, -1);

    }

    public void DefaultCardConf(GameObject cardToConfObj,Cards cardToConf)
    {
        cardToConfObj.transform.GetChild(0).GetChild(0).
            GetComponent<Image>().sprite = cardToConf.artwork;//card image holder

        cardToConfObj.transform.GetChild(1).GetChild(0).
            GetComponent<Text>().text = cardToConf.name;//card name holder
        if (cardToConf.cardType == GameConstant.AllCardType.Consumable)
        {
            cardToConfObj.transform.GetChild(2).GetChild(0).
                GetComponent<Text>().text ="Amou: "+cardToConf.amountEffect
                +"\nPer: "+cardToConf.percentageEffect*100+"%"
                +"\nSP: "+cardToConf.cost;//card stats holder
        }
        else
        {
            cardToConfObj.transform.GetChild(2).GetChild(0).
                GetComponent<Text>().text = " HP: " + cardToConf.hp + "\n SP: " + cardToConf.spPool +
                "\n P.ATK: " + cardToConf.atk + "\n M.ATK: " + cardToConf.matk +
                "\n P.DEF: " + cardToConf.def + "\n M.DEF: " + cardToConf.mdef +
                "\n Crit.Rate: " + cardToConf.critRate;//card stats holder
        }

        cardToConfObj.transform.GetChild(3).GetChild(0).
            GetComponent<Text>().text =""+ cardToConf.cardType;//card Effects description holder
            //GetComponent<Text>().text =""+ cardToConf.effects;//card Effects description holder
    }

    IEnumerator ReagengeCard(int antPos, int pos, string who = "Line", float signal = 1) //-1 é o de linha 1 é o de coluna
    {
        float posTemp = 0;
        float posX = player.transform.localPosition.x; //the position of the card on the x-axis
        float posY = player.transform.localPosition.y; //the position of the card on the y-axis
        int xPos = 0, yPos = 0; // the position, 0 is the min and 4 the max.
        int PosOfAnt = antPos; // the previous position of the tile

        GameConstant.TilesInZone card = null;
        GameObject cardObj = null;
        int randomIndex = PickRandomCardWithChanche();
        if (pityToUpLv >= 14) randomIndex = GenerateBoss();

        if (pos < antPos)
        {
            while (PosOfAnt < 4)
            {
                PosOfAnt += 1;
                switch (PosOfAnt)
                {
                    case 2:
                        posTemp = -walkRange * signal; //vai ir para a posição 1
                        break;
                    case 3:
                        posTemp = 0; //vai ir para a posição 2
                        break;
                    case 4:
                        posTemp = walkRange * signal; //vai ir para a posição 3
                        break;
                }
                if (who.Equals("Line"))
                { card = tilesInZone.Find(item => item.linePos == PosOfAnt && item.colPos == colPos); card.linePos = PosOfAnt - 1; posY = posTemp; }
                else { card = tilesInZone.Find(item => item.linePos == linePos && item.colPos == PosOfAnt); card.colPos = PosOfAnt - 1; posX = posTemp; }

                isReadjusting = true;
                yield return new WaitForSeconds(waitBetween);
                StartCoroutine(SeekTo(card.cardObject.transform, new Vector2(posX, posY), duration));
                while (isReadjusting) { yield return null; }
            }
            if (who.Equals("Line")) { xPos = 4; posY = -walkRange * 2; cardObj = CreateCard(posY: -345, posX: player.transform.localPosition.x); }
            else { yPos = 4; posX = walkRange * 2; cardObj = CreateCard(posX: 345, posY: player.transform.localPosition.y); }
        }
        else
        {
            while (PosOfAnt > 0)
            {
                PosOfAnt -= 1;

                switch (PosOfAnt)
                {
                    case 0:
                        posTemp = -walkRange * signal; // vai ir para a posição 1
                        break;
                    case 1:
                        posTemp = 0; // vai ir para a posição 2
                        break;
                    case 2:
                        posTemp = walkRange * signal; // vai ir para a posição 3
                        break;
                }

                if (who.Equals("Line"))
                { card = tilesInZone.Find(item => item.linePos == PosOfAnt && item.colPos == colPos); card.linePos = PosOfAnt + 1; posY = posTemp; }
                else { card = tilesInZone.Find(item => item.linePos == linePos && item.colPos == PosOfAnt); card.colPos = PosOfAnt + 1; posX = posTemp; }

                isReadjusting = true;
                yield return new WaitForSeconds(waitBetween);
                StartCoroutine(SeekTo(card.cardObject.transform, new Vector2(posX, posY), duration));
                while (isReadjusting) { yield return null; }
            }
            if (who.Equals("Line")) { xPos = 0; posY = walkRange * 2; cardObj = CreateCard(posY: 345, posX: player.transform.localPosition.x); }
            else { yPos = 0; posX = -walkRange * 2; cardObj = CreateCard(posX: -345, posY: player.transform.localPosition.y); }
        }

        if (who.Equals("Line")) { tilesInZone.Add(new GameConstant.TilesInZone(allCards[randomIndex], xPos, colPos, cardObj, lv)); posX = cardObj.transform.localPosition.x; }
        else { tilesInZone.Add(new GameConstant.TilesInZone(allCards[randomIndex], linePos, yPos, cardObj, lv)); posY = cardObj.transform.localPosition.y; }

        DefaultCardConf(cardObj, tilesInZone[tilesInZone.Count - 1].cards);

        isReadjusting = true;
        yield return new WaitForSeconds(waitBetween);
        StartCoroutine(SeekTo(cardObj.transform, new Vector2(posX, posY), duration));
        while (isReadjusting) { yield return null; }

    }

    IEnumerator AddAllCard()
    {
        float posY = 230f;
 
        for (int i = 0; i < 5; i++)
        {
            float posX = 230f;
            for (int j = 0; j < 5; j++)
            {
                if (i == 2 && j == 2) { }
                else
                {
                    GameObject cardObj = Instantiate(defaultCardPrefab);
                    int randomIndex = PickRandomCardWithChanche();
                    cardObj.transform.SetParent(allCardHolder);
                    cardObj.transform.localScale = Vector3.one;
                    cardObj.transform.localPosition = new Vector3(-345f, posY, 0);

                    tilesInZone.Add(new GameConstant.TilesInZone(allCards[randomIndex], i, 4-j, cardObj));
                    switch (allCards[randomIndex].cardType)
                    {
                        default:
                            DefaultCardConf(cardObj, tilesInZone[tilesInZone.Count-1].cards); 
                            break;
                    }
                    isReadjusting = true;
                    yield return new WaitForSeconds(waitBetween);
                    StartCoroutine(SeekTo(cardObj.transform, new Vector2(posX, posY), duration));
                    while (isReadjusting) { yield return null; }
                }
                posX -= 115f;
            }
            posY -= 115f;

        }
    }

    public GameObject CreateCard(float posY=230f,float posX= -345f)
    {
        GameObject cardObj = Instantiate(defaultCardPrefab);
        cardObj.transform.SetParent(allCardHolder);
        cardObj.transform.localScale = Vector3.one;
        cardObj.transform.localPosition = new Vector3(posX, posY, 0);
        return cardObj;
    }

    public int PickRandomCardWithChanche()
    {
        float randomChoose = Random.Range(0.0f,1.0f);
        int randomIndex = 0;
        Cards[] allCardsToUseTemp;

        float rarityChance = Random.Range(0.0f, 1.0f); //2% legendary  15% rare 83% common

        GameConstant.AllRarity rar = rarityChance <= 0.83 ? GameConstant.AllRarity.Common : rarityChance <= 0.98f ? GameConstant.AllRarity.Rare : GameConstant.AllRarity.Legendary; 
        GameConstant.AllRarity rarE = rarityChance <= 0.9 ? GameConstant.AllRarity.Common : GameConstant.AllRarity.Rare; //rarity for the enemies.

        
        if (randomChoose <= 0.69f) //69% chance of spawning enemies
        {
            allCardsToUseTemp = allCards.FindAll(item => item.cardType == GameConstant.AllCardType.Enemy && item.rarity == rarE).ToArray();
            randomIndex = allCards.IndexOf(allCardsToUseTemp[Random.Range(0,allCardsToUseTemp.Length)]);
        }
        else if (randomChoose <= 0.84) //15% equipment
        {
            allCardsToUseTemp = allCards.FindAll(item => item.cardType == GameConstant.AllCardType.Equipment && item.rarity == rar).ToArray();
            randomIndex = allCards.IndexOf(allCardsToUseTemp[Random.Range(0, allCardsToUseTemp.Length)]);
        }
        else //16% consumable
        {
            allCardsToUseTemp = allCards.FindAll(item => item.cardType == GameConstant.AllCardType.Consumable && item.rarity == rar).ToArray();
            randomIndex = allCards.IndexOf(allCardsToUseTemp[Random.Range(0, allCardsToUseTemp.Length)]);
        }
        return randomIndex;
    }

    public int GenerateBoss()
    {
        Cards[] allCardsToUseTemp = allCards.FindAll(item => item.rankOfEnemy == GameConstant.AllRanksOfEnemy.Boss).ToArray();
        return allCards.IndexOf(allCardsToUseTemp[Random.Range(0, allCardsToUseTemp.Length)]);
    }

    public void GenerateMiniOrBossDrop(Cards card,int xPos, int yPos, Transform cardTrans)
    {
        float randomChoose = Random.Range(0.0f,1.0f);
        int randomIndex = 0;
        if (randomChoose <= 0.9f) //90% rare = 0.9
        {
            Cards[] allCardsToUseTemp = allCards.FindAll(item => item.rarity == GameConstant.AllRarity.Rare&&
            item.cardType != GameConstant.AllCardType.Enemy).ToArray();
            randomIndex = allCards.IndexOf(allCardsToUseTemp[Random.Range(0, allCardsToUseTemp.Length)]);
        }
        else //10% legendary
        {
            Cards[] allCardsToUseTemp = allCards.FindAll(item => item.rarity == GameConstant.AllRarity.Legendary &&
            item.cardType != GameConstant.AllCardType.Enemy).ToArray();
            randomIndex = allCards.IndexOf(allCardsToUseTemp[Random.Range(0, allCardsToUseTemp.Length)]);
        }

        //Creating a drop with 2 or 4 level above the current.
        GameObject cardObj = CreateCard(posX: cardTrans.localPosition.x, posY: cardTrans.localPosition.y);
        switch (card.rankOfEnemy)
        {
            case GameConstant.AllRanksOfEnemy.MiniBoss:
                tilesInZone.Add(new GameConstant.TilesInZone(allCards[randomIndex], xPos, yPos, cardObj, lv+Random.Range(2,4)));
                DefaultCardConf(cardObj, tilesInZone[tilesInZone.Count - 1].cards);
                break;
            case GameConstant.AllRanksOfEnemy.Boss:
                tilesInZone.Add(new GameConstant.TilesInZone(allCards[randomIndex], xPos, yPos, cardObj, lv + Random.Range(4, 5)));
                DefaultCardConf(cardObj, tilesInZone[tilesInZone.Count - 1].cards);
                break;
            default:
                break;
        }
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("scene000");
    }

    public void ChangeMode(TextMeshProUGUI textField)
    {
        isInBattle = !isInBattle;
        if (!isInBattle) textField.text = "SCAN MODE";
        else textField.text = "BATTLE MODE";
    }

    public void ChangeFightStyle(TextMeshProUGUI textField)
    {
        untilTheEnd = !untilTheEnd;
        if (!untilTheEnd) textField.text = "Normal Fight";
        else textField.text = "Fight Until Dead";
    }

    public void InfoShow(int newLine, int newCol)
    {
        Cards card = null;
        if(newLine>=0&&newCol>=0) card = tilesInZone.Find(item => item.linePos == newLine && item.colPos == newCol).cards;
        if (card == null) card = playerMainCard;

        mainInfo.transform.GetChild(0).GetChild(0).GetComponent<Image>().
            sprite = card.artwork; //CardImage
        mainInfo.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().
            text = card.name;

        mainInfo.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().
            text =""+ card.cardType; //Card Type

        mainInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>()
            .text = "Class:\n"+card.cardClass; //Card Class
        mainInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>()
            .text = "Rarity:\n"+card.rarity; //Card Rarity

        for(int i = 0; i < contentInfoShow.transform.childCount; i++)
        {
            contentInfoShow.transform.GetChild(i).gameObject.SetActive(false);

        }
        switch (card.cardType)
        {
            case GameConstant.AllCardType.Consumable:
                contentInfoShow.transform.GetChild(6).gameObject.SetActive(true);
                contentInfoShow.transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text =
                    "<u>Amount Effect:</u>"+card.amountEffect+ "\n\n<u>Effect Percentage:</u>"+card.percentageEffect;
                contentInfoShow.transform.GetChild(7).gameObject.SetActive(true);
                break;
            default:
                contentInfoShow.transform.GetChild(0).gameObject.SetActive(true);
                contentInfoShow.transform.GetChild(0).GetChild(0).
                    GetComponent<TextMeshProUGUI>().text = "<u>HP:</u>" + card.hp + "\n\n<u>ATK:</u>" + card.atk
                    + "\n\n<u>SP:</u>" + card.spPool + "\n\n<u>M.ATK:</u>" + card.matk + "\n\n<u>DEF:</u>" + card.def +
                    "<u>\n\nM.DEF:</u>" + card.mdef + "\n\n<u>CRIT.RATE:</u>" + card.critRate
                    + "\n\n<u>EVAS.RATE:</u>" + card.evasion;
                contentInfoShow.transform.GetChild(1).gameObject.SetActive(true);
                break;
        }
        if(card.cardType == GameConstant.AllCardType.Enemy)
        {
            contentInfoShow.transform.GetChild(2).gameObject.SetActive(true);
            contentInfoShow.transform.GetChild(2).GetChild(0).
                    GetComponent<TextMeshProUGUI>().text =""+ card.rankOfEnemy;
            contentInfoShow.transform.GetChild(3).gameObject.SetActive(true);
        }
        if(card.cardType != GameConstant.AllCardType.Consumable)
        {
            contentInfoShow.transform.GetChild(4).gameObject.SetActive(true);
            contentInfoShow.transform.GetChild(4).GetChild(0).
                GetComponent<TextMeshProUGUI>().text = "" + card.attackType;
            contentInfoShow.transform.GetChild(5).gameObject.SetActive(true);
        }
        contentInfoShow.transform.GetChild(8).gameObject.SetActive(true);
        contentInfoShow.transform.GetChild(8).GetChild(0).GetComponent<TextMeshProUGUI>().text = "EFFECTS:\n";
        for (int i = 0; i < card.effects.Length; i++)
        {
            contentInfoShow.transform.GetChild(8).gameObject.SetActive(true);
            contentInfoShow.transform.GetChild(8).GetChild(0).GetComponent<TextMeshProUGUI>().text += ""+card.effects[i];
        }
        contentInfoShow.transform.GetChild(9).gameObject.SetActive(true);
    }
}
