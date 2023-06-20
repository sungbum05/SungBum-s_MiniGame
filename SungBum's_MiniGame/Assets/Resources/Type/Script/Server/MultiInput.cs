using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MultiInput : MonoBehaviour
{
    public TMP_InputField MyInputField;
    public TMP_InputField OtherInputField;

    public void EnterAnswer()
    {
        TypeServerManager.Instance.TypeClient.Send($"&ANSWER|{MyInputField.text}");
    }

    public void EndAnswer()
    {
        if (MyInputField.text == TypeServerManager.Instance.TypeClient.Question)
        {
            TypeServerManager.Instance.TypeClient.MyScore += 100;
            TypeServerManager.Instance.TypeClient.ScoreSetting();

            TypeServerManager.Instance.TypeClient.Send($"&OTHERSCORE");

            TypeServerManager.Instance.TypeServer.StartQuestion();

        }

        TypeServerManager.Instance.MultiInput.MyInputField.ActivateInputField();
        MyInputField.text = "";
    }

    public void OtherAnswer(string str)
    {
        OtherInputField.text = str;
    }
}
