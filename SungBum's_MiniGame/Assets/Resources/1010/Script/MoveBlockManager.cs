using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlockManager : MonoBehaviour
{
    public GameObject SelectMoveBlock;

    [SerializeField] HoldBlockManager HoldBlockManager;

    [SerializeField] GameObject SelectHoldBlock;
    [SerializeField] Color SelectHoldBlockColor;

    [SerializeField] int SelectHoldBlockNumber;

    [SerializeField] List<MoveBlock> MoveBlockList;

    [SerializeField] LayerMask HoldBlockLayer;
    [SerializeField] LayerMask MoveBlockLayer;

    int[,] ReciveBlockData = new int[5, 5]
    {
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 }
    };

    private void Awake()
    {
        HoldBlockManager = GameObject.FindObjectOfType<HoldBlockManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ResearchHoldBlock();
        MoveBlockDrag();
        CheckingAllDropBlock();
    }

    void CheckingAllDropBlock()
    {
        for (int i = 0; i < MoveBlockList.Count; i++)
        {
            if (MoveBlockList[i].DropBlock == false)
                return;
        }

        for (int i = 0; i < MoveBlockList.Count; i++)
        {
            MoveBlockList[i].ResetUpBlock();
        }
    }

    void ResearchHoldBlock()
    {
        Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D HoldBlockHit = Physics2D.Raycast(MousePosition, transform.forward, 10.0f, HoldBlockLayer);
        Debug.DrawRay(MousePosition, transform.forward * 10.0f, Color.red, 0.3f);

        if (HoldBlockHit && Input.GetMouseButtonUp(0) && SelectMoveBlock != null)
        {
            //Debug.Log(hit.transform.gameObject.GetComponent<HoldBlock>().HoldBlockNum);

            SelectHoldBlock = HoldBlockHit.transform.gameObject;
            SelectHoldBlockNumber = SelectHoldBlock.GetComponent<HoldBlock>().HoldBlockNum;

            HoldBlockManager.DropMoveBlock(ReciveBlockData, SelectHoldBlockNumber, SelectHoldBlockColor, SelectMoveBlock);
        }
    }

    void MoveBlockDrag()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D MoveBlockHit = Physics2D.Raycast(MousePosition, transform.forward, 10.0f, MoveBlockLayer);
            Debug.DrawRay(MousePosition, transform.forward * 10.0f, Color.red, 0.3f);

            if (MoveBlockHit) // 문제점 여기 수정
            {
                SelectMoveBlock = MoveBlockHit.transform.gameObject;

                SelectMoveBlock.transform.position = new Vector3(MousePosition.x, MousePosition.y, -1);
                SelectMoveBlock.transform.localScale = Vector3.one;

                ReciveData();
            }
        }

        else if(SelectMoveBlock != null && Input.GetMouseButtonUp(0))
        {
            SelectMoveBlock.transform.position = SelectMoveBlock.GetComponent<MoveBlock>().OriginalVector;
            SelectMoveBlock.transform.localScale = Vector3.one / 2;

            SelectMoveBlock = null;
        }
    }

    void ReciveData()
    {
        SelectHoldBlockColor = SelectMoveBlock.GetComponent<MoveBlock>().SendColorData;
        ReciveBlockData = SelectMoveBlock.GetComponent<MoveBlock>().MgrSendBlockBox;
    }
}
