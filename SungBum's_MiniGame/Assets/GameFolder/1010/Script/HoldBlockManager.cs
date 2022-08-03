using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldBlockManager : MonoBehaviour
{
    [Header("HoldBlockManager_속성")]
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

    void HoldBlockBasicSetting() // 기초적인 HoldBlock 번호 세팅을 해준다
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
        } // HoldBlock 리스트 추가 및 번호 설정
    }

    public void FillColor(int[,] FillblockData, int BlockNumber, Color BlockColor) // HoldBlock의 색깔을 드랍한 블럭의 위치와 색깔과 같게 해준다.
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
