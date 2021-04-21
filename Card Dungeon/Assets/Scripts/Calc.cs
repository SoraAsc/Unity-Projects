using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calc : MonoBehaviour
{
    public static int DamageCalc(Cards atacante,Cards defensor, int critical)
    {
        switch (atacante.attackType)
        {
            case GameConstant.AllAttacksTypes.Physical:
                return (atacante.atk/defensor.def)*critical > 0 ? (atacante.atk / defensor.def) * critical : 1;               
            default:
                return (atacante.matk / defensor.mdef)*critical > 0 ? (atacante.matk / defensor.mdef) * critical : 1;
        }
        
    }
}
