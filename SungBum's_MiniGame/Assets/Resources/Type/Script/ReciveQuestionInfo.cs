using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Collections;
using System.Runtime.InteropServices.WindowsRuntime;

public class ReciveQuestionInfo : MonoBehaviour
{
    [System.Serializable]
    public class QuestionDatas
    {
        public QuestionInfo[] QuestionData;
    }

    [SerializeField]
    public QuestionDatas DataList;

    void Awake()
    {
        string JsonData = Resources.Load<TextAsset>("Type/Data/QuestionData").ToString();

        DataList = JsonUtility.FromJson<QuestionDatas>(JsonData);
    }

    public QuestionInfo[] GetQuestionData()
    {
        return DataList.QuestionData;
    }
}
