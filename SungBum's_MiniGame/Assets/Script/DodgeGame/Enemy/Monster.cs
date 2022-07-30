using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EnemyTierList // ���� Ƽ��
{
    Common = 4, 
    Rare = 3, 
    Epic = 2, 
    Legendary = 1
}

[System.Serializable]
public class Monster : MonoBehaviour
{
    [Header("����_���� �Ӽ�")]
    [SerializeField] private int hp; // ������Ƽ���� Ȯ���ϱ� ����
    public int HP { 
        get 
        { 
            return hp; 
        }

        set
        {
            hp = value;

            if(hp <= 0)
                Debug.Log("Die");
        }
    }
    [SerializeField] protected DodgeGameManager gameManager;
    [SerializeField] private EnemyTierList EnemyTier;

    [SerializeField] protected GameObject EnemyBullet;
    [SerializeField] protected int AttackPower = 0;
    [SerializeField] protected float MaxAttakDelay = 1f;
    [SerializeField] protected float CurAttakDelay = 0f;

    protected virtual void Awake()
    {
        UnitSetting();
        StartCoroutine(AttckPatton());
    }

    protected virtual void UnitSetting()
    {
        gameManager = GameObject.FindObjectOfType<DodgeGameManager>();
        HP = 100 / (int)EnemyTier;
        AttackPower = 100 / (int)EnemyTier;

        CurAttakDelay = MaxAttakDelay;
    }

    protected virtual IEnumerator AttckPatton()
    {
        yield return null;
    }
}
