using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class GameConstant
{
    public enum AllParts
    {
        NONE,
        BASE,
        ARM,
        BOOT,
        BODYARMOR,
    }
    public enum AllStates
    {
        NONE,
        IDLE,
        IDLESHOT,
        WALK,
        WALKSHOT,
        JUMP,
        JUMPSHOT,
        FALL,
        FALLSHOT,
        FALLRECOVERY,
        FALLRECOVERYSHOT,
        HURTH,
        DEATH,
        WALLSLIDE,
        WALLSLIDESHOT,
        WALLSLIDEJUMP,
    }
}


