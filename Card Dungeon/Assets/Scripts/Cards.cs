using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class Cards : ScriptableObject
{
    public string id;
    public new string name;

    public Sprite artwork;

    public GameConstant.AllCardType cardType;
    public GameConstant.AllClass cardClass;
    public GameConstant.AllAttacksTypes attackType;
    public GameConstant.AllRarity rarity;
    public GameConstant.AllRanksOfEnemy rankOfEnemy;

    public string position;

    #region Attributes
    public int hp;

    public int atk;
    public int matk;

    public int def;
    public int mdef;

    public float critRate;
    public float evasion;

    public int spPool;
    #endregion

    #region Effects
    public GameConstant.AllEffects[] effects;
    //public GameConstant.AllEffects[] effectList;
    public int amountEffect;
    public float percentageEffect;
    public int cost;
    #endregion


}

#if UNITY_EDITOR
[CustomEditor(typeof(Cards))]
class CardEditor : Editor
{
    Cards card = null;
    private void OnEnable()
    {
        //Liga as propriedades
        card = (Cards)target;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (card == null) return;

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("ID:",GUILayout.Width(50));
        card.id = EditorGUILayout.TextField(card.id);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Nome:", GUILayout.Width(50));
        card.name = EditorGUILayout.TextField(card.name);
        EditorGUILayout.EndHorizontal();

        card.artwork =(Sprite) EditorGUILayout.ObjectField("Artwork:", card.artwork, typeof(Sprite),false);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Type:", GUILayout.Width(50));
        card.cardType = (GameConstant.AllCardType)  EditorGUILayout.EnumPopup(card.cardType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("CLASS:");
        card.cardClass = (GameConstant.AllClass)EditorGUILayout.EnumPopup(card.cardClass);
        if (card.cardType == GameConstant.AllCardType.Enemy) card.rankOfEnemy = (GameConstant.AllRanksOfEnemy)EditorGUILayout.EnumPopup(card.rankOfEnemy);


        SerializedProperty effects = serializedObject.FindProperty("effects");
        switch (card.cardType)
        {
            case GameConstant.AllCardType.Consumable:

                EditorGUILayout.PropertyField(effects, true);

                EditorGUILayout.LabelField("AMOUNT EFFECT:");
                card.amountEffect = EditorGUILayout.IntField(card.amountEffect);
                if (card.amountEffect <= 0)
                {
                    EditorGUILayout.LabelField("EFFECT PERCENTAGE:");
                    card.percentageEffect = EditorGUILayout.Slider(card.percentageEffect, 0.00f, 1.00f);
                }
                EditorGUILayout.LabelField("Cost:");
                card.cost = EditorGUILayout.IntField(card.cost);
                break;
            case GameConstant.AllCardType.None:
                break;

            default:
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("HP:", GUILayout.Width(50));
                card.hp = EditorGUILayout.IntField(card.hp);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("SP:", GUILayout.Width(50));
                card.spPool = EditorGUILayout.IntField(card.spPool);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("P.ATK:", GUILayout.Width(50));
                card.atk = EditorGUILayout.IntField(card.atk);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("M.ATK:", GUILayout.Width(50));
                card.matk = EditorGUILayout.IntField(card.matk);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("P.DEF:", GUILayout.Width(50));
                card.def = EditorGUILayout.IntField(card.def);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("M.DEF:", GUILayout.Width(50));
                card.mdef = EditorGUILayout.IntField(card.mdef);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Crit.Rate:", GUILayout.Width(60));
                card.critRate = EditorGUILayout.Slider(card.critRate,0.00f,1.00f);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Evas.Rate:", GUILayout.Width(60));
                card.evasion = EditorGUILayout.Slider(card.evasion, 0.00f, 1.00f);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.PropertyField(effects, true);

                EditorGUILayout.LabelField("Attack Type:");
                card.attackType = (GameConstant.AllAttacksTypes)EditorGUILayout.EnumPopup(card.attackType);


                if (card.effects != null)
                {
                    if (card.effects.Length>0)//card.effects[0] != GameConstant.AllEffects.None)
                    {
                        EditorGUILayout.LabelField("AMOUNT EFFECT:");
                        card.amountEffect = EditorGUILayout.IntField(card.amountEffect);
                        if (card.amountEffect <= 0)
                        {
                            EditorGUILayout.LabelField("EFFECT PERCENTAGE:");
                            card.percentageEffect = EditorGUILayout.Slider(card.percentageEffect, 0.00f, 1.00f);
                        }
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Cost:", GUILayout.Width(50));
                        card.cost = EditorGUILayout.IntField(card.cost);
                        EditorGUILayout.EndHorizontal();
                    }
                }
                break;
        }
        EditorGUILayout.LabelField("Rarity:");
        card.rarity = (GameConstant.AllRarity)EditorGUILayout.EnumPopup(card.rarity);

        bool somethingChanged = EditorGUI.EndChangeCheck();
        if (somethingChanged) EditorUtility.SetDirty(card);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif