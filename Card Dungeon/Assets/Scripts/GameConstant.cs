using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstant : MonoBehaviour
{
    public enum AllCardType
    {
        None,
        Player,
        Enemy,
        Equipment,
        Consumable
    }

    public enum AllClass
    {
        None,
        Mage,
        Warrior,
        Undead,
        Berserker,
        Ninja,
        Necromancer
    }

    public enum AllEffects
    {
        None,
        RecoverHp,
        RecoverMana,
        ChangeAttackType,
    }

    public enum AllRanksOfEnemy
    {
        None,
        Commoner,
        MiniBoss,
        Boss,
    }

    public enum AllRarity
    {
        None,
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
    }

    public enum AllAttacksTypes
    {
        None,
        Physical,
        Magical,
    }

    [System.Serializable]
    public class Tile
    {
        public GameObject lineTile;
        public int linePos;

        public List<GameObject> colTile;
        public List<int> colPos;
        public Tile(GameObject newLineTile)
        {
            if (newLineTile != null)
            {
                lineTile = newLineTile;
                linePos = int.Parse(lineTile.name);

                colTile = new List<GameObject>(lineTile.transform.childCount);
                colPos = new List<int>(lineTile.transform.childCount);

                for(int i = 0; i < lineTile.transform.childCount; i++)
                {
                    colTile.Add(lineTile.transform.GetChild(i).gameObject);
                    colPos.Add(int.Parse(lineTile.transform.GetChild(i).name));
                }
            }
        }
    }
    [System.Serializable]
    public class TilesInZone
    {
        public GameObject cardObject;
        public Cards cards;
        public int linePos;
        public int colPos;

        public TilesInZone(Cards newCard, int newLinePos, int newColPos, GameObject newCardObject, int lv=1)
        {
            cards = Instantiate(newCard);
            linePos = newLinePos;
            colPos = newColPos;
            cardObject = newCardObject;

            cards.amountEffect *= lv;
            cards.atk *= lv;
            cards.matk *= lv;
            cards.mdef *= lv;
            cards.def *= lv;
            cards.cost *= lv;
            cards.hp *= lv;
        }
    }
    public enum Directions
    {
        None,
        Up,
        Down,
        Right,
        Left
    }

    public static Cards CardStatusTransfer(Cards oldCard, Cards newCard)
    {
        newCard = Instantiate(newCard);
        newCard.hp += oldCard.hp;
        newCard.spPool += oldCard.spPool;
        newCard.atk += oldCard.atk;
        newCard.matk += oldCard.matk;
        newCard.def += oldCard.def;
        newCard.mdef += oldCard.mdef;
        newCard.effects = oldCard.effects;

        return newCard;
    }

}
