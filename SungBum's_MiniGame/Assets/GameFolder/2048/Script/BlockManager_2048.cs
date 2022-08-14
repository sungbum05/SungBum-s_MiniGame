using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MoveAxis
{
    Horizontal, Vertical
}

public class BlockManager_2048 : MonoBehaviour
{
    const int BLOCK_SIZE = 4;
    const int START_BLOCK_COUNT = 2;

    [SerializeField] GameObject Board;
    [SerializeField] GameObject[,] Blocks = new GameObject[BLOCK_SIZE, BLOCK_SIZE];

    [SerializeField] List<GameObject> ContainNumberBlockList;
    [SerializeField] List<GameObject> NoneNumberBlockList;

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
            MoveBlockValue(MoveAxis.Horizontal, 1);
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveBlockValue(MoveAxis.Horizontal, -1);
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveBlockValue(MoveAxis.Vertical, 1);
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveBlockValue(MoveAxis.Vertical, -1);
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
                NoneNumberBlockList.Add(Block.gameObject);

                Block.GetComponent<HoldBlock_2048>().BlockSetting(BlockNum);

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

            ContainNumberBlockList.Add(NoneNumberBlockList[RandomNumber]);

            NoneNumberBlockList[RandomNumber].GetComponent<HoldBlock_2048>().SetValue(RandomValue());
            NoneNumberBlockList.RemoveAt(RandomNumber);
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

    void MoveBlockValue(MoveAxis Axis, int Dir)
    {
        int MoveLinevalue = Dir == 1 ? Blocks.GetLength(0) : 0;
        int BorderValue = Dir == 1 ? 0 : Blocks.GetLength(0);
        int AddValue = Dir == 1 ? 1 : 0;

        int TestValue = 1;

        if (Axis.Equals(MoveAxis.Horizontal)) // 새로 줄 이동
        {
            for (int i = 0; i < Blocks.GetLength(0); i++)
            {
                for (int j = MoveLinevalue; j != BorderValue; j += (-1 * Dir))
                {
                    Debug.Log("J");
                    Debug.Log(j - AddValue);

                    Blocks[i, j - AddValue].GetComponent<HoldBlock_2048>().SetValue(TestValue);
                    TestValue++;
                }
            }
        }

        else // 가로 줄 이동
        {
            for (int i = 0; i < Blocks.GetLength(1); i++)
            {
                for (int j = MoveLinevalue; j != BorderValue; j += (-1 * Dir))
                {
                    Debug.Log("J");
                    Debug.Log(j - AddValue);

                    Blocks[j - AddValue, i].GetComponent<HoldBlock_2048>().SetValue(TestValue);
                    TestValue++;
                }
            }
        }
    }
}
