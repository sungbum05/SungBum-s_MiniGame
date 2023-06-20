using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeChat : MonoBehaviour
{
    public static TypeChat Instance;
    void Awake() => Instance = this;

    public void ShowMessage(string data)
    {
        Debug.Log(data);
    }
}
