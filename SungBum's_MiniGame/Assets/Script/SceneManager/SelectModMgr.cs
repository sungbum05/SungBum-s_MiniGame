using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
class GameModInfo
{
    [Header("게임 버튼 속성")]
    public string GameName;
    public int GameNumber;
    [SerializeField]
    string SceneName;

    [Header("게임 설명 내용")]
    public int GameModScore;
}

enum FontStyle
{
    Nomal, Bold, Italic, Bold_Italic
}


public class SelectModMgr : MonoBehaviour
{
    [Header("게임 모드 내부 속성")]
    [SerializeField]
    List <GameModInfo> gameModInfos = new List <GameModInfo> ();

    [SerializeField]
    GameObject AddGameModArea = null;
    [SerializeField]
    float GameModAreaSize = 200;
    [SerializeField]
    int MaxLineContents = 3;
    [SerializeField]
    float PlusLineHeight = 220;
    [SerializeField]
    GameObject GameModButton = null;

    [Header("게임모드 버튼 속성")]
    [SerializeField]
    int FontSize = 15;
    [SerializeField]
    FontStyle FontStyle = FontStyle.Nomal;

    private void Awake()
    {
        AddGameModSystem();
    }

    void AddGameModSystem()
    {
        int GameModNumber = 1;

        foreach (GameModInfo Info in gameModInfos)
        {
            #region 버튼 생성
            GameObject Button = Instantiate(GameModButton);
            Button.transform.SetParent(AddGameModArea.transform);
            #endregion

            #region 버튼 외관 설정
            Button.transform.localScale = Vector3.one;
            Button.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

            if(GameModNumber != 1 && (GameModNumber - 1) % MaxLineContents == 0)
            {
                AddGameModArea.GetComponent<RectTransform>().sizeDelta += new Vector2(0, PlusLineHeight);
            }
            #endregion

            #region 버튼 내부 설정
            Info.GameNumber = GameModNumber;

            Button.transform.GetChild(0).GetComponent<Text>().text = $"{Info.GameNumber}.{Info.GameName}";
            Button.transform.GetChild(0).GetComponent<Text>().fontSize = FontSize;
            Button.transform.GetChild(0).GetComponent<Text>().fontStyle =  UnityEngine.FontStyle.Normal;
            #endregion

            #region 버튼 텍스트 설정

            #endregion

            GameModNumber++;
        }
    }

    void InfoEnterButton()
    {

    }
}
