using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstant : MonoBehaviour
{
    public enum GridDirection
    {
        Line,
        Col,
    }

    public enum AllPowerUps
    {
        None,
        BombNum,
        BombPower,
        WalkSpeed,
    }

    public static float appearPowerUpRate = 0.8f;
}
