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
        MyInputField.text = "";
    }

    public void OtherAnswer(string str)
    {
        OtherInputField.text = str;
    }
}
