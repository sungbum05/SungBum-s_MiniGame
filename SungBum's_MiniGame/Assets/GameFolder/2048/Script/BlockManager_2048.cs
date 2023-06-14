using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager_2048 : MonoBehaviour
{
    const int BLOCK_SIZE = 4;
    const int START_BLOCK_COUNT = 2;
    const int CUR_BLOCK_COUNT = 1;

    [SerializeField] GameObject Board;
    [SerializeField] GameObject[,] Blocks = new GameObject[BLOCK_SIZE, BLOCK_SIZE];

    public List<GameObject> ContainNumberBlockList;
    public List<GameObject> NoneNumberBlockList;

    [SerializeField] List<Color> Colors = new List<Color>();

    // Start is called before the first frame update
    void Start()
    {
        AddBlocks();
        BoradSetting();
    }

    // Update is called once per frame
    void Update()
    {
        InputKey();
    }

    void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(HorizontalMove(1));
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(HorizontalMove(-1));
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(VerticalMove(-1));
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(VerticalMove(1));
        }
    }

    void AddBlocks()
    {
        int BlockNum = 0;
        int i = 0, j = 0;

        foreach (Transform Line in Board.transform)
        {
            foreach (Transform Block in Line.transform)
            {
                Blocks[i, j] = Block.gameObject;
                Block.gameObject.name = $"{Block.name}_{BlockNum}";

                Block.GetComponent<HoldBlock_2048>().BlockSetting(BlockNum);

                Block.gameObject.GetComponent<HoldBlock_2048>().ResetValue();

                j++;
                BlockNum++;
            }

            j = 0;
            i++;
        }
    }

    void BoradSetting()
    {
        for (int i = 0; i < START_BLOCK_COUNT; i++)
        {
            int RandomNumber = Random.Range(0, NoneNumberBlockList.Count);
            Debug.Log(RandomNumber);

            NoneNumberBlockList[RandomNumber].GetComponent<HoldBlock_2048>().SetValue(RandomValue());
        }
    }

    int RandomValue()
    {
        int value = 0;

        int RanValue = Random.Range(0, 10);

        if (RanValue < 9)
            value = 2;

        else
            value = 4;

        return value;
    }

    void SpawnBlock()
    {
        for (int i = 0; i < CUR_BLOCK_COUNT; i++)
        {
            int RandomNumber = Random.Range(0, NoneNumberBlockList.Count);
            Debug.Log(RandomNumber);

            NoneNumberBlockList[RandomNumber].GetComponent<HoldBlock_2048>().SetValue(RandomValue());
        }
    }

    bool CanHorizontalMoveCheck(int x, int y, int MoveValue, int Dir)
    {
        if(Dir > 0)
        {
            if(Blocks[x, y + MoveValue + Dir].GetComponent<HoldBlock_2048>().BlockValue > 0)
            {
                return true;
            }

            return false;
        }

        else
        {
            if (Blocks[x, y - MoveValue + Dir].GetComponent<HoldBlock_2048>().BlockValue > 0)
            {
                return true;
            }

            return false;
        }
    }

    bool CanVerticalMoveCheck(int x, int y, int MoveValue, int Dir)
    {
        if (Dir > 0)
        {
            if (Blocks[y + MoveValue + Dir, x].GetComponent<HoldBlock_2048>().BlockValue > 0)
            {
                return true;
            }

            return false;
        }

        else
        {
            if (Blocks[y - MoveValue + Dir, x].GetComponent<HoldBlock_2048>().BlockValue > 0)
            {
                return true;
            }

            return false;
        }
    }

    IEnumerator HorizontalMove(int Dir)
    {
        yield return null;

        int MoveLinevalue = Dir == 1 ? Blocks.GetLength(0) : 0;
        int BorderValue = Dir == 1 ? 0 : Blocks.GetLength(0);
        int AddValue = Dir == 1 ? 1 : 0;

        for (int i = 0; i < Blocks.GetLength(0); i++)
        {
            yield return null;

            for (int j = MoveLinevalue; j != BorderValue; j += (-1 * Dir))
            {
                yield return null;

                int MoveToPos = 0;

                int BlockNum = j - AddValue;

                bool MixedValue = false;

                if (Blocks[i, (j - AddValue)].GetComponent<HoldBlock_2048>().BlockValue > 0)
                {
                    int SendValue = Blocks[i, (j - AddValue)].GetComponent<HoldBlock_2048>().BlockValue;
                    Blocks[i, (j - AddValue)].GetComponent<HoldBlock_2048>().ResetValue();

                    while (true)
                    {
                        yield return null;

                        if (Dir == 1)
                        {
                            if (BlockNum < 3 && Blocks[i, (j - AddValue) + MoveToPos + Dir].GetComponent<HoldBlock_2048>().BlockValue == SendValue && MixedValue == false) // 합쳐지는 코드
                            {
                                MixedValue = true;

                                SendValue += SendValue;
                                Blocks[i, (j - AddValue) + MoveToPos + Dir].GetComponent<HoldBlock_2048>().ResetValue();
                            }

                            if (BlockNum >= 3 || CanHorizontalMoveCheck(i, j - AddValue, MoveToPos, Dir)) // 가는방향에 블럭이 있거나 끝이면 멈추는 코드
                            {
                                Blocks[i, (j - AddValue) + MoveToPos].GetComponent<HoldBlock_2048>().SetValue(SendValue);
                                break;
                            }

                            MoveToPos++;

                            BlockNum++;
                            Debug.Log(BlockNum);
                        }

                        else
                        {
                            if (BlockNum > 0 && Blocks[i, (j - AddValue) - MoveToPos + Dir].GetComponent<HoldBlock_2048>().BlockValue == SendValue && MixedValue == false) // 합쳐지는 코드
                            {
                                MixedValue = true;

                                SendValue += SendValue;
                                Blocks[i, (j - AddValue) - MoveToPos + Dir].GetComponent<HoldBlock_2048>().ResetValue();
                            }

                            if (BlockNum <= 0 || CanHorizontalMoveCheck(i, j - AddValue, MoveToPos, Dir)) // 가는방향에 블럭이 있거나 끝이면 멈추는 코드
                            {
                                Blocks[i, (j - AddValue) - MoveToPos].GetComponent<HoldBlock_2048>().SetValue(SendValue);
                                break;
                            }

                            MoveToPos++;

                            BlockNum--;
                            Debug.Log(BlockNum);
                        }
                    }
                }

                //Blocks[i, j - AddValue].GetComponent<HoldBlock_2048>().SetValue(j - AddValue);
            }
        }

        SpawnBlock();
        Debug.Log("END");
    }

    IEnumerator VerticalMove(int Dir)
    {
        yield return null;

        int MoveLinevalue = Dir == 1 ? Blocks.GetLength(0) : 0;
        int BorderValue = Dir == 1 ? 0 : Blocks.GetLength(0);
        int AddValue = Dir == 1 ? 1 : 0;

        for (int i = 0; i < Blocks.GetLength(1); i++)
        {
            yield return null;

            for (int j = MoveLinevalue; j != BorderValue; j += (-1 * Dir))
            {
                yield return null;

                int MoveToPos = 0;

                int BlockNum = j - AddValue;

                bool MixedValue = false;

                if (Blocks[(j - AddValue), i].GetComponent<HoldBlock_2048>().BlockValue > 0)
                {
                    int SendValue = Blocks[(j - AddValue), i].GetComponent<HoldBlock_2048>().BlockValue;
                    Blocks[(j - AddValue), i].GetComponent<HoldBlock_2048>().ResetValue();

                    while (true)
                    {
                        yield return null;

                        if (Dir == 1)
                        {
                            if (BlockNum < 3 && Blocks[(j - AddValue) + MoveToPos + Dir, i].GetComponent<HoldBlock_2048>().BlockValue == SendValue && MixedValue == false) // 합쳐지는 코드
                            {
                                MixedValue = true;

                                SendValue += SendValue;
                                Blocks[(j - AddValue) + MoveToPos + Dir, i].GetComponent<HoldBlock_2048>().ResetValue();
                            }

                            if (BlockNum >= 3 || CanVerticalMoveCheck(i, j - AddValue, MoveToPos, Dir))
                            {
                                Blocks[(j - AddValue) + MoveToPos, i].GetComponent<HoldBlock_2048>().SetValue(SendValue);
                                break;
                            }

                            MoveToPos++;

                            BlockNum++;
                            Debug.Log(BlockNum);
                        }

                        else
                        {
                            if (BlockNum > 0 && Blocks[(j - AddValue) - MoveToPos + Dir, i].GetComponent<HoldBlock_2048>().BlockValue == SendValue && MixedValue == false) // 합쳐지는 코드
                            {
                                MixedValue = true;

                                SendValue += SendValue;
                                Blocks[(j - AddValue) - MoveToPos + Dir, i].GetComponent<HoldBlock_2048>().ResetValue();
                            }

                            if (BlockNum <= 0 || CanVerticalMoveCheck(i, j - AddValue, MoveToPos, Dir))
                            {
                                Blocks[(j - AddValue) - MoveToPos, i].GetComponent<HoldBlock_2048>().SetValue(SendValue);
                                break;
                            }

                            MoveToPos++;

                            BlockNum--;
                            Debug.Log(BlockNum);
                        }
                    }
                }
                //Blocks[j - AddValue, i].GetComponent<HoldBlock_2048>().SetValue(TestValue);
            }
        }

        SpawnBlock();
    }
}
