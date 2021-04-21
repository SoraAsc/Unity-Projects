using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int numLine;
    int numCol;
    List<GameConstant.AllCardTypes> cardTypes;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        SceneManager.LoadScene("sceneTheme");
    }

    public void SetValues(int newNumLine, int newNumCol, List<GameConstant.AllCardTypes> newCardTypes)
    {
        numLine = newNumLine;
        numCol = newNumCol;
        cardTypes = newCardTypes;
    }

    public (int,int,List<GameConstant.AllCardTypes>) GetValues()
    {
        return (numLine, numCol, cardTypes);
    }
}
