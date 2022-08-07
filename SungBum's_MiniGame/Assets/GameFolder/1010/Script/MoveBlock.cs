using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    [SerializeField] ArrayList MoveBlockTypeList = new ArrayList();

    [SerializeField] Transform BlockZipTransform;
    [SerializeField] GameObject[,] MoveBlockList = new GameObject[5, 5];

    //BlockList
    int[,,] MoveBlockBox = new int[4, 5, 5];

    public int[,] MgrSendBlockBox  = new int[5, 5];

    #region 1x4
    int[,,] b_1x4 = new int[4, 5, 5]
    { { // 0
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 1, 1, 1, 1 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { //90
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { // 180
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 1, 1, 1, 1 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { // 270
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 0, 0, 0 }
    } };

    //������
    //������
    //������
    //������
    //������
    #endregion

    #region 1x5
    int[ , , ] b_1x5 = new int[4, 5, 5]
    { { // 0
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 1, 1, 1, 1, 1 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { //90
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 }
    },
    { // 180
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 1, 1, 1, 1, 1 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { // 270
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 }
    } };

    //������
    //������
    //������
    //������
    //������
    #endregion

    private void Start()
    {
        AddBlockList();
        MoveBlockBasicSetting();
        RandomDrawBlock();

        PrintBlock();
    }

    void AddBlockList() // ���� Ÿ���� �߰�
    {
        MoveBlockTypeList.Add(b_1x4);
        MoveBlockTypeList.Add(b_1x5);        
    }

    void MoveBlockBasicSetting() // �������� MoveBlock�� MoveBlockList���ٰ� �߰����ش�
    {
        int x = 0, y = 0;

        foreach (Transform MoveBlocks in BlockZipTransform)
        {
            foreach (Transform MoveBlock in MoveBlocks)
            {
                MoveBlockList[x, y] = MoveBlock.gameObject;

                y++;
            }

            y = 0;
            x++;
        } // HoldBlock ����Ʈ �߰� �� ��ȣ ����
    }

    void RandomDrawBlock() // ������ �� ������ �̱�
    {
        int MoveBlockType = Random.Range(0, MoveBlockTypeList.Count);
        MoveBlockBox = (int[,,])MoveBlockTypeList[MoveBlockType];

        int RotationType = Random.Range(0, 4);

        for (int i = 0; i < MoveBlockBox.GetLength(1); i++)
        {
            for (int j = 0; j < MoveBlockBox.GetLength(2); j++)
            {
                MgrSendBlockBox[i, j] = MoveBlockBox[RotationType, i, j];
            }
        }
    }

    void PrintBlock() // ���� �� ������ ���
    {
        Debug.Log(MgrSendBlockBox[0, 2]);

        for (int i = 0; i < MoveBlockList.GetLength(0); i++)
        {
            for (int j = 0; j < MoveBlockList.GetLength(1); j++)
            {
                if (MgrSendBlockBox[i, j] == 1)
                {
                    MoveBlockList[i, j].GetComponent<SpriteRenderer>().color = Color.red;
                }

                else
                {
                    Color color = MoveBlockList[i, j].GetComponent<SpriteRenderer>().color;
                    color.a = 0.0f;

                    MoveBlockList[i, j].GetComponent<SpriteRenderer>().color = color;
                }
            }
        }
    }
}
