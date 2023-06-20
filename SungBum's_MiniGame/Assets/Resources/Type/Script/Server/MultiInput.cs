using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiInput : MonoBehaviour
{
    public InputField MyInputField;
    public InputField OtherInputField;

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
