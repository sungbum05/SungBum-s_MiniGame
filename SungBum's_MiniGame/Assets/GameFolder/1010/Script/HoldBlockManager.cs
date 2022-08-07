using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldBlockManager : MonoBehaviour
{
    [Header("HoldBlockManager_속성")]
    [SerializeField] Transform BlockZipTransform;

    [SerializeField] GameObject [ , ] HoldBlockList = new GameObject [10, 10];
    [SerializeField] GameObject SelectHoldBlock;

    [SerializeField] bool IsHorizontalLineFill = false, IsVerticalLineFill = false;
    [SerializeField] List<Vector2> ClearLineList; 

    [SerializeField] int Score = 0;

    private void Awake()
    {
        HoldBlockBasicSetting();
    }

    private void Update()
    {
        LineClear();
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

    void AddScore(int PlusScore)
    {
        Score += PlusScore;
    } // 점수 추가

    void HorizontalLineCheck() // 가로 라인 체크
    {
        for (int i = 0; i < HoldBlockList.GetLength(0); i++)
        {
            bool LineCheck = true;

            for (int j = 0; j < HoldBlockList.GetLength(1); j++)
            {
                if (HoldBlockList[i, j].GetComponent<HoldBlock>().IsFill == false)
                {
                    LineCheck = false;
                    break;
                }
            }

            if (LineCheck == true)
            {
                ClearLineList.Add(new Vector2(i + 1, 0));
            }
        }

        IsHorizontalLineFill = true;
    }

    void VerticalLineCheck() // 세로 라인 체크
    {
        for (int i = 0; i < HoldBlockList.GetLength(1); i++)
        {
            bool LineCheck = true;

            for (int j = 0; j < HoldBlockList.GetLength(0); j++)
            {
                if (HoldBlockList[j, i].GetComponent<HoldBlock>().IsFill == false)
                {
                    LineCheck = false;
                    break;
                }
            }

            if (LineCheck == true)
            {
                ClearLineList.Add(new Vector2(0, i + 1));
            }
        }

        IsVerticalLineFill = true;
    }

    void LineClear() // 체크를 통해 받아온 라인 클리어
    {
        if (IsHorizontalLineFill && IsVerticalLineFill)
        {
            IsHorizontalLineFill = false;
            IsVerticalLineFill = false;

            Debug.Log("Clear");

            if(ClearLineList.Count > 0)
            {
                Debug.Log("Not Null");

                for (int i = 0; i < 2; i++)
                {
                    Debug.Log(i);

                    if(ClearLineList[0].x > 0) // 가로 라인 클리어
                    {
                        for (int j = 0; j < HoldBlockList.GetLength(1); j++)
                        {
                            HoldBlockList[(int)ClearLineList[0].x - 1, j].GetComponent<SpriteRenderer>().color = Color.white;
                            HoldBlockList[(int)ClearLineList[0].x - 1, j].GetComponent<HoldBlock>().IsFill = false;

                            AddScore(1);
                        }
                    }

                    else // 세로 라인 클리어
                    {
                        for (int j = 0; j < HoldBlockList.GetLength(0); j++)
                        {
                            Debug.Log(ClearLineList[0]);

                            HoldBlockList[j, (int)ClearLineList[0].y - 1].GetComponent<SpriteRenderer>().color = Color.white;
                            HoldBlockList[j, (int)ClearLineList[0].y - 1].GetComponent<HoldBlock>().IsFill = false;

                            AddScore(1);
                        }
                    }

                    ClearLineList.RemoveAt(0);
                }
            }
        }
    }

    bool CheckFillHoldBlock(int[,] FillblockData, int BlockNumber) // 블럭의 예외처리 담당(채워져 있거나 정해진 칸을 벗어났던가)
    {
        for (int i = 0; i < FillblockData.GetLength(0); i++)
        {
            for (int j = 0; j < FillblockData.GetLength(1); j++)
            {
                if (FillblockData[i, j] == 1)
                {
                    int FrontArrayNumber = (BlockNumber / 10) + (i - (FillblockData.GetLength(0) / 2));
                    int BackArrayNumber = (BlockNumber % 10) + (j - (FillblockData.GetLength(1) / 2));

                    if ((FrontArrayNumber < 0 || BackArrayNumber < 0) || (FrontArrayNumber > HoldBlockList.GetLength(0) - 1 || BackArrayNumber > HoldBlockList.GetLength(1) - 1)) // 블럭이 정해진 칸보다 왼쪽에 있던가 오른쪽에 있던가
                    {
                        return true;
                    }

                    SelectHoldBlock = HoldBlockList[FrontArrayNumber, BackArrayNumber];

                    if (SelectHoldBlock.GetComponent<HoldBlock>().IsFill == true) // 이미 차있는 공간에 블럭이 들어 갈 때
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public void DropMoveBlock(int[,] FillblockData, int BlockNumber, Color BlockColor) // HoldBlock의 색깔을 드랍한 블럭의 위치와 색깔과 같게 해준다.
    {
        int PlusScore = 0;

        if (CheckFillHoldBlock(FillblockData, BlockNumber))
        {
            Debug.Log("Fill");
            return;
        }

        for (int i = 0; i < FillblockData.GetLength(0); i++)
        {
            for (int j = 0; j < FillblockData.GetLength(1); j++)
            {
                if(FillblockData[i,j] == 1)
                {
                    int FrontArrayNumber = (BlockNumber / 10) + (i - (FillblockData.GetLength(0) / 2));
                    int BackArrayNumber = (BlockNumber % 10) + (j - (FillblockData.GetLength(1) / 2));

                    SelectHoldBlock = HoldBlockList[FrontArrayNumber, BackArrayNumber];

                    SelectHoldBlock.GetComponent<SpriteRenderer>().color = BlockColor;
                    SelectHoldBlock.GetComponent<HoldBlock>().IsFill = true;

                    PlusScore++;
                }
            }
        }

        HorizontalLineCheck();
        VerticalLineCheck();

        AddScore(PlusScore);
    }
}
