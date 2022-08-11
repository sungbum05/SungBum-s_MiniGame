using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager_2048 : MonoBehaviour
{
    const int BLOCK_SIZE = 4;
    const int START_BLOCK_COUNT = 2;

    [SerializeField] GameObject Board;
    [SerializeField] GameObject[,] Blocks = new GameObject[BLOCK_SIZE, BLOCK_SIZE];

    // Start is called before the first frame update
    void Start()
    {
        AddBlocks();
        BoradSetting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddBlocks()
    {
        int BlockNum = 0;
        int i = 0, j = 0;

        foreach(Transform Line in Board.transform)
        {
            foreach(Transform Block in Line.transform)
            {
                Blocks[i, j] = Block.gameObject;
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
            int x = Random.Range(0, BLOCK_SIZE), y = Random.Range(0, BLOCK_SIZE);
            Debug.Log($"{x},{y}");

            Blocks[x, y].GetComponent<HoldBlock_2048>().SetValue(1);
        }
    }
}
