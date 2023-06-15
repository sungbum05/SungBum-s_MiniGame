using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeGameManager : MonoBehaviour
{
    [Header("내부 변수"), SerializeField]
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
    [SerializeField]
    private int ClearCount = 0;

    [Header("외부 변수-컴포넌트")]
    public Image GaugeImage;
    public GameObject StartPanel;

    [Header("외부 변수-시스템")]
    public InputSystem InputSystem;
    public QuestionSystem QuestionSystem;

    private void Update()
    {
        if(DecreaseSpeed > 0)
        {
            CurrentGauge -= DecreaseSpeed * Time.deltaTime;
            GaugeImage.fillAmount = CurrentGauge / MaxGauge;

            if (CurrentGauge <= 0)
                GameOver();
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

    public void GameOver()
    {
        StartPanel.SetActive(true);
        DecreaseSpeed = 0.0f;

        MaxGauge = 100;
        CurrentGauge = MaxGauge;
        GaugeImage.fillAmount = CurrentGauge / MaxGauge;
    }

    public void AnswerSetting(string Answer)
    {
        InputStr = Answer;

        if(InputStr == QuestionStr)
        {
            CurrentGauge = MaxGauge;
            ClearCount++;
            DecreaseSpeed = 2.0f + (ClearCount / 2.0f);

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
