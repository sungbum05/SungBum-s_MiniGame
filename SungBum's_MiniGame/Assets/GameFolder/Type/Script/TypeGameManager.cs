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

    [Header("외부 변수")]
    public Image GaugeImage;
    public GameObject StartPanel;


    /// <summary>
    /// Game Start Button Function
    /// </summary>
    public void GameStart()
    {
        StartPanel.SetActive(false);
        MaxGauge = 100;
        CurrentGauge = MaxGauge;
    }
}
