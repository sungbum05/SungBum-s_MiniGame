using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlockManager : MonoBehaviour
{
    [SerializeField] HoldBlockManager HoldBlockManager;
    [SerializeField] GameObject SelectHoldBlock;
    [SerializeField] Color SelectHoldBlockColor;

    [SerializeField] int SelectHoldBlockNumber;

    int[,] TestBlock = new int[5, 5]
    {
     { 0, 0, 0, 0, 0 },
     { 0, 0, 0, 0, 0 },
     { 0, 0, 1, 0, 0 },
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
    }

    void ResearchHoldBlock()
    {
        Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(MousePosition, transform.forward, 10.0f);
        Debug.DrawRay(MousePosition, transform.forward * 10.0f, Color.red, 0.3f);

        if (hit && Input.GetMouseButtonUp(0))
        {
            //Debug.Log(hit.transform.gameObject.GetComponent<HoldBlock>().HoldBlockNum);

            SelectHoldBlock = hit.transform.gameObject;
            SelectHoldBlockNumber = SelectHoldBlock.GetComponent<HoldBlock>().HoldBlockNum;

            HoldBlockManager.DropMoveBlock(TestBlock, SelectHoldBlockNumber, SelectHoldBlockColor);

        }
    }
}
