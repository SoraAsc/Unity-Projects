using UnityEngine;

[CreateAssetMenu (fileName ="newCard",menuName = "Card")]
public class Card : ScriptableObject
{
    public string id;
    public new string name;
    public Sprite artwork;
    public GameConstant.AllCardTypes cardType;
    [TextArea(2,6)]
    public string credits;
}
