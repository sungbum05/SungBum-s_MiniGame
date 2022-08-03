using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldBlockManager : MonoBehaviour
{
    [Header("HoldBlockManager_속성")]
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

    public void ChangeColor(int BlockNumber, Color BlockColor, int Up, int Down, int Left, int Right) // HoldBlock의 색깔을 드랍한 블럭의 위치와 색깔과 같게 해준다.
    {
        HoldBlockList[BlockNumber / 10, BlockNumber % 10].GetComponent<SpriteRenderer>().color = BlockColor;
        HoldBlockList[BlockNumber / 10 - Up, BlockNumber % 10].GetComponent<SpriteRenderer>().color = BlockColor;
        HoldBlockList[BlockNumber / 10 + Down, BlockNumber % 10].GetComponent<SpriteRenderer>().color = BlockColor;
        HoldBlockList[BlockNumber / 10, BlockNumber % 10 - Left].GetComponent<SpriteRenderer>().color = BlockColor;
        HoldBlockList[BlockNumber / 10, BlockNumber % 10 + Right].GetComponent<SpriteRenderer>().color = BlockColor;
    }
}
