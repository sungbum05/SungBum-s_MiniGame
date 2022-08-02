using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlockManager : MonoBehaviour
{
    [SerializeField] int Up, Down, Left, Right;

    [SerializeField] GameObject SelectHoldBlock;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(MousePosition, transform.forward, 10.0f);
        Debug.DrawRay(MousePosition, transform.forward * 10.0f, Color.red, 0.3f);
        if(hit)
        {
            Debug.Log(hit.transform.gameObject.GetComponent<HoldBlock>().HoldBlockNum);
        }
    }   
}
