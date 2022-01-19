using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName ="newStateAnimationFrame", menuName ="State Animation Frame")]
public class StateAnimationFrame : ScriptableObject
{
    [Header("Arm")]
    public GameConstant.AllParts armPart;
    public GameConstant.AllStates armState;
    public Sprite[] armSpriteFrame;

    [Header("Boot")]
    public GameConstant.AllParts bootPart;
    public GameConstant.AllStates bootstate;
    public Sprite[] bootSpriteFrame;

    [Header("Body")]
    public GameConstant.AllParts bodyPart;
    public GameConstant.AllStates bodystate;
    public Sprite[] bodySpriteFrame;

}
