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

    void HoldBlockBasicSetting()
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

        HoldBlockList[0, 0].GetComponent<SpriteRenderer>().color = Color.red;
        HoldBlockList[1, 0].GetComponent<SpriteRenderer>().color = Color.red;
        HoldBlockList[2, 0].GetComponent<SpriteRenderer>().color = Color.blue;
        HoldBlockList[3, 0].GetComponent<SpriteRenderer>().color = Color.cyan;
        HoldBlockList[4, 0].GetComponent<SpriteRenderer>().color = Color.red;
    }
}
