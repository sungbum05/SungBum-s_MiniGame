using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionSystem : MonoBehaviour
{
    [Header("���� ����")]
    public QuestionInfo[] QuestionArray;

    [Header("�ܺ� ����")]
    public TextMeshProUGUI QuestionText;
    public TypeGameManager GameManager;
    public ReciveQuestionInfo ReciveQuestionInfo;

    public void GameStart(TypeGameManager MainManager)
    {
        GameManager = MainManager;
        QuestionArray = ReciveQuestionInfo.GetQuestionData();

        MakeQuestion();
    }

    public void MakeQuestion()
    {
        int QuestionNum = Random.Range(0, QuestionArray.Length);

        GameManager.QuestionSetting(QuestionArray[QuestionNum].Questions);
        QuestionText.text = QuestionArray[QuestionNum].Questions;
    }
}
