using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldBlock_2048 : MonoBehaviour
{
    [Space(10)] public int BlockNumber;

    [Space(10)] public int BlockValue = 0;
    [SerializeField] Text BlockText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValue(int Value)
    {
        BlockValue = Value;

        if (BlockValue <= 0)
        {
            BlockText.text = null;
        }

        else
        {
            BlockText.text = BlockValue.ToString();
        }
    }

    public void BlockSetting(int Number)
    {
        BlockNumber = Number;

        BlockText = this.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
    }

}
