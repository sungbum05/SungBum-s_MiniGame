using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldBlockManager : MonoBehaviour
{
    [Header("HoldBlockManager_�Ӽ�")]
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

    void AddScore(int PlusScore)
    {
        Score += PlusScore;
    } // ���� �߰�

    void HorizontalLineCheck() // ���� ���� üũ
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

    void VerticalLineCheck() // ���� ���� üũ
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

    void LineClear() // üũ�� ���� �޾ƿ� ���� Ŭ����
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

                    if(ClearLineList[0].x > 0) // ���� ���� Ŭ����
                    {
                        for (int j = 0; j < HoldBlockList.GetLength(1); j++)
                        {
                            HoldBlockList[(int)ClearLineList[0].x - 1, j].GetComponent<SpriteRenderer>().color = Color.white;
                            HoldBlockList[(int)ClearLineList[0].x - 1, j].GetComponent<HoldBlock>().IsFill = false;

                            AddScore(1);
                        }
                    }

                    else // ���� ���� Ŭ����
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

    bool CheckFillHoldBlock(int[,] FillblockData, int BlockNumber) // ���� ����ó�� ���(ä���� �ְų� ������ ĭ�� �������)
    {
        for (int i = 0; i < FillblockData.GetLength(0); i++)
        {
            for (int j = 0; j < FillblockData.GetLength(1); j++)
            {
                if (FillblockData[i, j] == 1)
                {
                    int FrontArrayNumber = (BlockNumber / 10) + (i - (FillblockData.GetLength(0) / 2));
                    int BackArrayNumber = (BlockNumber % 10) + (j - (FillblockData.GetLength(1) / 2));

                    if ((FrontArrayNumber < 0 || BackArrayNumber < 0) || (FrontArrayNumber > HoldBlockList.GetLength(0) - 1 || BackArrayNumber > HoldBlockList.GetLength(1) - 1)) // ���� ������ ĭ���� ���ʿ� �ִ��� �����ʿ� �ִ���
                    {
                        return true;
                    }

                    SelectHoldBlock = HoldBlockList[FrontArrayNumber, BackArrayNumber];

                    if (SelectHoldBlock.GetComponent<HoldBlock>().IsFill == true) // �̹� ���ִ� ������ ���� ��� �� ��
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public void DropMoveBlock(int[,] FillblockData, int BlockNumber, Color BlockColor) // HoldBlock�� ������ ����� ���� ��ġ�� ����� ���� ���ش�.
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
