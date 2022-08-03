using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldBlockManager : MonoBehaviour
{
    [Header("HoldBlockManager_�Ӽ�")]
    [SerializeField] Transform BlockZipTransform;

    [SerializeField] GameObject [ , ] HoldBlockList = new GameObject [10, 10];
    [SerializeField] GameObject SelectHoldBlock;

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

    public void FillColor(int[,] FillblockData, int BlockNumber, Color BlockColor) // HoldBlock�� ������ ����� ���� ��ġ�� ����� ���� ���ش�.
    {
        //HoldBlockList[BlockNumber / 10, BlockNumber % 10].GetComponent<SpriteRenderer>().color = BlockColor;
        for (int i = 0; i < FillblockData.GetLength(0); i++)
        {
            for (int j = 0; j < FillblockData.GetLength(1); j++)
            {
                if(FillblockData[i,j] == 1)
                {
                    SelectHoldBlock = HoldBlockList[(BlockNumber / 10) + (i - (FillblockData.GetLength(0) / 2)), (BlockNumber % 10) + (j - (FillblockData.GetLength(1) / 2))];

                    SelectHoldBlock.GetComponent<SpriteRenderer>().color = BlockColor;
                }
            }
        }
    }
}
