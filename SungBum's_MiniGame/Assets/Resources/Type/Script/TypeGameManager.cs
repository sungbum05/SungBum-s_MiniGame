using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeGameManager : MonoBehaviour
{
    [Header("���� ����"), SerializeField]
    private float currentgauge;
    public float CurrentGauge
    {
        get
        { 
            return currentgauge; 
        }
        set
        {
            currentgauge = value; 
        }
    }
    [SerializeField]
    private float MaxGauge = 100;
    [SerializeField]
    private float DecreaseSpeed;
    [SerializeField]
    private string QuestionStr;
    [SerializeField]
    private string InputStr;

    [Header("�ܺ� ����-������Ʈ")]
    public Image GaugeImage;
    public GameObject StartPanel;

    [Header("�ܺ� ����-�ý���")]
    public InputSystem InputSystem;
    public QuestionSystem QuestionSystem;

    private void Update()
    {
        if(DecreaseSpeed > 0)
        {
            CurrentGauge -= DecreaseSpeed * Time.deltaTime;
            GaugeImage.fillAmount = CurrentGauge / MaxGauge;
        }
    }

    /// <summary>
    /// Game Start Button Function
    /// </summary>
    public void GameStart()
    {
        StartPanel.SetActive(false);
        MaxGauge = 100;
        CurrentGauge = MaxGauge;
        DecreaseSpeed = 2.0f;

        InputSystem.GameStart(this);
        QuestionSystem.GameStart(this);
    }

    public void AnswerSetting(string Answer)
    {
        InputStr = Answer;

        if(InputStr == QuestionStr)
        {
            CurrentGauge = MaxGauge;
            QuestionSystem.MakeQuestion();
        }

        else
        {
            CurrentGauge -= 10;
        }
    }    

    public void QuestionSetting(string Question)
    {
        QuestionStr = Question;
    }
}