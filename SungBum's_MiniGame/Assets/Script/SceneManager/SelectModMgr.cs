using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
class GameModInfo
{
    [Header("���� ��ư �Ӽ�")]
    public string GameName;
    public int GameNumber;
    [SerializeField]
    string SceneName;

    [Header("���� ���� ����")]
    public int GameModScore;
}

enum FontStyle
{
    Nomal, Bold, Italic, Bold_Italic
}


public class SelectModMgr : MonoBehaviour
{
    [Header("���� ��� ���� �Ӽ�")]
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

    [Header("���Ӹ�� ��ư �Ӽ�")]
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
            #region ��ư ����
            GameObject Button = Instantiate(GameModButton);
            Button.transform.SetParent(AddGameModArea.transform);
            #endregion

            #region ��ư �ܰ� ����
            Button.transform.localScale = Vector3.one;
            Button.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

            if(GameModNumber != 1 && (GameModNumber - 1) % MaxLineContents == 0)
            {
                AddGameModArea.GetComponent<RectTransform>().sizeDelta += new Vector2(0, PlusLineHeight);
            }
            #endregion

            #region ��ư ���� ����
            Info.GameNumber = GameModNumber;

            Button.transform.GetChild(0).GetComponent<Text>().text = $"{Info.GameNumber}.{Info.GameName}";
            Button.transform.GetChild(0).GetComponent<Text>().fontSize = FontSize;
            Button.transform.GetChild(0).GetComponent<Text>().fontStyle =  UnityEngine.FontStyle.Normal;
            #endregion

            #region ��ư �ؽ�Ʈ ����

            #endregion

            GameModNumber++;
        }
    }

    void InfoEnterButton()
    {

    }
}
