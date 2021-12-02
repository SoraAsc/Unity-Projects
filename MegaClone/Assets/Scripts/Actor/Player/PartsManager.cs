using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remover membros privados não utilizados", Justification = "To avoid warnings in private methods provided by Unity.")]
public class PartsManager : MonoBehaviour
{
    private AnimatorController[] aniController; // Helmet/BodyBase = [Lvs: 0 = Base | 1 = upgrade 1 | 2 = upgrade 2 | 3 = upgrade 3 ]
    [SerializeField]
    private SpriteRenderer[] playerParts; // [Pos: 0 = Arm | 1 = Boot | 2 = Body ]
    [SerializeField]
    [Range(0,3)]
    private int[] lv;  // [Pos: 0 = Arm | 1 = Boot | 2 = Body | 3 = BodyBase]  ,[Lvs: 0 = Base | 1 = upgrade 1 | 2 = upgrade 2 | 3 = upgrade 3 ]
    [SerializeField]
    private bool[] equippedPart; //[Pos: 0 = Arm | 1 = Boot | 2 = Body | 3 = BodyBase] 

    private void ChangeParts(StateAnimationFrame stateAnimationFrame)
    {
        if (lv[0] > 0 && equippedPart[0] && stateAnimationFrame.armPart == GameConstant.AllParts.ARM) { ChangeBoots(stateAnimationFrame.armSpriteFrame[lv[0] - 1], 0 ); }
        else { playerParts[0].enabled = false; }

        if (lv[1] > 0 && equippedPart[1] && stateAnimationFrame.bootPart == GameConstant.AllParts.BOOT) { ChangeBoots( stateAnimationFrame.bootSpriteFrame[lv[1]-1] ); }
        else { playerParts[1].enabled = false; }

        if (lv[2] > 0 && equippedPart[2] && stateAnimationFrame.bodyPart == GameConstant.AllParts.BODYARMOR) { ChangeBoots(stateAnimationFrame.bodySpriteFrame[lv[2] - 1], 2); }
        else { playerParts[2].enabled = false; }
    }

    private void ChangeBoots(Sprite sprite, int partIndex = 1)
    {
        playerParts[partIndex].enabled = true;
        playerParts[partIndex].sprite = sprite;
    }

}
