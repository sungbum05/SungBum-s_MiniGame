using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionSystem : MonoBehaviour
{
    [Header("내부 변수")]
    public QuestionInfo[] QuestionArray;

    [Header("외부 변수")]
    public TextMeshProUGUI QuestionText;
    public TypeGameManager GameManager;
    public ReciveQuestionInfo ReciveQuestionInfo;

    public void Start()
    {
        QuestionArray = ReciveQuestionInfo.GetQuestionData();
    }

    public void MakeQuestion()
    {
        int QuestionNum = Random.Range(0, QuestionArray.Length);

        QuestionText.text = QuestionArray[QuestionNum].Questions;
        StartCoroutine(DelayQuestion(QuestionNum));

        Debug.Log("MakeQuestion");
    }

    IEnumerator DelayQuestion(int Num)
    {
        yield return new WaitForSeconds(0.05f);
        TypeServerManager.Instance.TypeClient.Send($"&QUESTION|{QuestionArray[Num].Questions}");
    }
}
