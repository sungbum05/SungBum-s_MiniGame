using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputSystem : MonoBehaviour
{
    [Header("외부 변수")]
    public InputField AnswerInputField;
    public TypeGameManager GameManager;

    public void GameStart(TypeGameManager MainManager)
    {
        GameManager = MainManager;
        AnswerInputField.Select();
    }

    /// <summary>
    /// Enter Answer Function
    /// </summary>
    public void EnterAnswer()
    {
        GameManager.AnswerSetting(AnswerInputField.text);

        AnswerInputField.text = null;
        AnswerInputField.ActivateInputField();
    }
}
