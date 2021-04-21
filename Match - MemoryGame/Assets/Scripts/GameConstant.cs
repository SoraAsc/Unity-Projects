using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstant : MonoBehaviour
{
    public enum AllCardTypes
    {
        None,
        Fruit,
        Number,
    }

    public class CardInfo
    {
        public Card card;
        public int max = 2; //The max num of combinations.
    }

    public class CardGame
    {
        public Card card;
        public CardBehaviour behaviour;
    }

}
