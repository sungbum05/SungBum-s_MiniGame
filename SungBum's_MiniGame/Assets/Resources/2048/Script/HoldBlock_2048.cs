using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldBlock_2048 : MonoBehaviour
{
    [Space(10)] public int BlockNumber;

    [Space(10)] public int BlockValue = 0;
    [SerializeField] Text BlockText;

    [SerializeField] BlockManager_2048 blockManager;


    void Awake()
    {
        blockManager = GameObject.FindObjectOfType<BlockManager_2048>();
    }


    public void BlockSetting(int Number)
    {
        BlockNumber = Number;

        BlockText = this.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
    }

    public void ResetValue()
    {
        BlockValue = 0;

        if (blockManager.ContainNumberBlockList.Contains(this.gameObject) == true)
            blockManager.ContainNumberBlockList.Remove(this.gameObject);

        if (blockManager.NoneNumberBlockList.Contains(this.gameObject) == false)
            blockManager.NoneNumberBlockList.Add(gameObject);

        BlockText.text = "";
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
            if (blockManager.ContainNumberBlockList.Contains(this.gameObject) == false)
                blockManager.ContainNumberBlockList.Add(this.gameObject);

            if (blockManager.NoneNumberBlockList.Contains(this.gameObject) == true)
                blockManager.NoneNumberBlockList.Remove(this.gameObject);

            BlockText.text = BlockValue.ToString();
        }
    }
}
