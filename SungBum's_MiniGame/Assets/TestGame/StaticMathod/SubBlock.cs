using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubBlock : MonoBehaviour
{
    public int Hp = 100;
    static int H_p = 100;

    public SubBlock()
    {
        Debug.Log("������ ȣ��");
        MainBlock.BlockCount++;
    }

    public static void LongAttack()
    {
        H_p -= 10;
    }
}
