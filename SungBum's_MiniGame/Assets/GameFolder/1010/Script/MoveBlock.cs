using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    public Vector3 OriginalVector;
    public Color SendColorData;

    public int[,] MgrSendBlockBox = new int[5, 5];

    public bool AllDiffrentBlockDrop = true;
    public bool DropBlock = true;

    [SerializeField] MoveBlockManager MoveBlockManager;

    [SerializeField] ArrayList MoveBlockTypeList = new ArrayList();
    [SerializeField] List<Color> BlockColorList;

    [SerializeField] Transform BlockZipTransform;
    [SerializeField] GameObject[,] MoveBlockList = new GameObject[5, 5];

    //BlockList
    int[,,] MoveBlockBox = new int[4, 5, 5];

    #region Small_L
    int[,,] b_Small_L = new int[4, 5, 5]
    { { // 0
     { 0, 0, 0, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 1, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { //90
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 1, 1, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { // 180
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 1, 1, 0, 0  },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { // 270
     { 0, 0, 0, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 1, 1, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 }
    } };

    //□□□□□
    //□□■□□
    //□□■■□
    //□□□□□
    //□□□□□
    #endregion

    #region Big_L
    int[,,] b_Big_L = new int[4, 5, 5]
    { { // 0
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 1, 1 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { //90
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 1, 1, 1 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 }
    },
    { // 180
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 1, 1, 1, 0, 0  },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 }
    },
    { // 270
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 1, 1, 1, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 }
    } };

    //□□■□□
    //□□■□□
    //□□■■■
    //□□□□□
    //□□□□□
    #endregion

    #region 1x1
    int[,,] b_1x1 = new int[4, 5, 5]
    { { // 0
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { //90
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { // 180
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 1, 0, 0  },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { // 270
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 }
    } };

    //□□□□□
    //□□□□□
    //□□■□□
    //□□□□□
    //□□□□□
    #endregion

    #region 1x2
    int[,,] b_1x2 = new int[4, 5, 5]
    { { // 0
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 1, 1, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { //90
     { 0, 0, 0, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { // 180
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 1, 1, 0  },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { // 270
     { 0, 0, 0, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 }
    } };

    //□□□□□
    //□□□□□
    //□□■■□
    //□□□□□
    //□□□□□
    #endregion

    #region 1x3
    int[,,] b_1x3 = new int[4, 5, 5]
    { { // 0
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 1, 1, 1, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { //90
     { 0, 0, 0, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { // 180
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 1, 1, 1, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { // 270
     { 0, 0, 0, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 1, 0, 0 },
     { 0, 0, 0, 0, 0 }
    } };

    //□□□□□
    //□□□□□
    //□■■■□
    //□□□□□
    //□□□□□
    #endregion

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

    //□□□□□
    //□□□□□
    //□■■■■
    //□□□□□
    //□□□□□
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

    //□□□□□
    //□□□□□
    //■■■■■
    //□□□□□
    //□□□□□
    #endregion

    #region 2x2
    int[,,] b_2x2 = new int[4, 5, 5]
    { { // 0
     { 0, 0, 0, 0, 0 },
     { 0, 0, 1, 1, 0 },
     { 0, 0, 1, 1, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { //90
     { 0, 0, 0, 0, 0 },
     { 0, 0, 1, 1, 0 },
     { 0, 0, 1, 1, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { // 180
     { 0, 0, 0, 0, 0 },
     { 0, 0, 1, 1, 0 },
     { 0, 0, 1, 1, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { // 270
     { 0, 0, 0, 0, 0 },
     { 0, 0, 1, 1, 0 },
     { 0, 0, 1, 1, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 }
    } };

    //□□□□□
    //□□■■□
    //□□■■□
    //□□□□□
    //□□□□□
    #endregion

    #region 3x3
    int[,,] b_3x3 = new int[4, 5, 5]
    { { // 0
     { 0, 0, 0, 0, 0 },
     { 0, 1, 1, 1, 0 },
     { 0, 1, 1, 1, 0 },
     { 0, 1, 1, 1, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { //90
     { 0, 0, 0, 0, 0 },
     { 0, 1, 1, 1, 0 },
     { 0, 1, 1, 1, 0 },
     { 0, 1, 1, 1, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { // 180
     { 0, 0, 0, 0, 0 },
     { 0, 1, 1, 1, 0 },
     { 0, 1, 1, 1, 0 },
     { 0, 1, 1, 1, 0 },
     { 0, 0, 0, 0, 0 }
    },
    { // 270
     { 0, 0, 0, 0, 0 },
     { 0, 1, 1, 1, 0 },
     { 0, 1, 1, 1, 0 },
     { 0, 1, 1, 1, 0 },
     { 0, 0, 0, 0, 0 }
    } };

    //□□□□□
    //□■■■□
    //□■■■□
    //□■■■□
    //□□□□□
    #endregion

    private void Start()
    {
        ResetUpBlock();
    }

    private void Update()
    {
        DiffrentBlockSelect();
    }

    public void ResetUpBlock()
    {
        DropBlock = false;
        this.gameObject.SetActive(true);

        AddBlockList();
        MoveBlockBasicSetting();
        RandomDrawBlock();
    }

    void DiffrentBlockSelect()
    {
        if (MoveBlockManager.SelectMoveBlock != null && MoveBlockManager.SelectMoveBlock.name != this.gameObject.name)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }

        else
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    

    void AddBlockList() // 블럭의 타입을 추가
    {
        MoveBlockTypeList.Add(b_Small_L);
        BlockColorList.Add(Color.yellow);

        MoveBlockTypeList.Add(b_Big_L);
        BlockColorList.Add(Color.green);

        MoveBlockTypeList.Add(b_1x1);
        BlockColorList.Add(Color.cyan);

        MoveBlockTypeList.Add(b_1x2);
        BlockColorList.Add(Color.grey);

        MoveBlockTypeList.Add(b_1x3);
        BlockColorList.Add(new Color(0,0.7f,0.7f,1));

        MoveBlockTypeList.Add(b_1x4);
        BlockColorList.Add(Color.red);

        MoveBlockTypeList.Add(b_1x5);
        BlockColorList.Add(Color.blue);

        MoveBlockTypeList.Add(b_2x2);
        BlockColorList.Add(new Color(0, 0, 0.7f, 1));

        MoveBlockTypeList.Add(b_3x3);
        BlockColorList.Add(new Color(0.7f, 0, 0, 1));
    }

    void MoveBlockBasicSetting() // 기초적인 MoveBlock을 MoveBlockList에다가 추가해준다
    {
        OriginalVector = this.gameObject.transform.position;

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
        } // HoldBlock 리스트 추가 및 번호 설정
    }

    void RandomDrawBlock() // 랜덤한 블럭 데이터 뽑기
    {
        int MoveBlockType = Random.Range(0, MoveBlockTypeList.Count);
        MoveBlockBox = (int[,,])MoveBlockTypeList[MoveBlockType];

        SendColorData = BlockColorList[MoveBlockType];

        int RotationType = Random.Range(0, 4);

        for (int i = 0; i < MoveBlockBox.GetLength(1); i++)
        {
            for (int j = 0; j < MoveBlockBox.GetLength(2); j++)
            {
                MgrSendBlockBox[i, j] = MoveBlockBox[RotationType, i, j];
            }
        }

        PrintBlock(MoveBlockType);
    }

    void PrintBlock(int ColorNumber) // 뽑은 블럭 데이터 출력
    {

        for (int i = 0; i < MoveBlockList.GetLength(0); i++)
        {
            for (int j = 0; j < MoveBlockList.GetLength(1); j++)
            {
                if (MgrSendBlockBox[i, j] == 1)
                {
                    MoveBlockList[i, j].GetComponent<SpriteRenderer>().color = BlockColorList[ColorNumber];
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
