using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldBlockManager : MonoBehaviour
{
    [Header("HoldBlockManager_�Ӽ�")]
    [SerializeField] Transform BlockZipTransform;

    [SerializeField] GameObject [ , ] HoldBlockList = new GameObject [10, 10];

    private void Awake()
    {
        HoldBlockBasicSetting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HoldBlockBasicSetting() // �������� HoldBlock ��ȣ ������ ���ش�
    {
        int x = 0, y = 0;

        int AddHoldBlockNum = 0;

        foreach(Transform HoldBlocks in BlockZipTransform)
        {
            foreach(Transform HoldBlock in HoldBlocks)
            {
                HoldBlock.GetComponent<HoldBlock>().HoldBlockNum = AddHoldBlockNum;

                HoldBlockList[x, y] = HoldBlock.gameObject;

                AddHoldBlockNum++;
                y++;
            }

            y = 0;
            x++;
        } // HoldBlock ����Ʈ �߰� �� ��ȣ ����
    }

    public void ChangeColor(int BlockNumber, Color BlockColor, int Up, int Down, int Left, int Right) // HoldBlock�� ������ ����� ���� ��ġ�� ����� ���� ���ش�.
    {
        HoldBlockList[BlockNumber / 10, BlockNumber % 10].GetComponent<SpriteRenderer>().color = BlockColor;
        HoldBlockList[BlockNumber / 10 - Up, BlockNumber % 10].GetComponent<SpriteRenderer>().color = BlockColor;
        HoldBlockList[BlockNumber / 10 + Down, BlockNumber % 10].GetComponent<SpriteRenderer>().color = BlockColor;
        HoldBlockList[BlockNumber / 10, BlockNumber % 10 - Left].GetComponent<SpriteRenderer>().color = BlockColor;
        HoldBlockList[BlockNumber / 10, BlockNumber % 10 + Right].GetComponent<SpriteRenderer>().color = BlockColor;
    }
}
