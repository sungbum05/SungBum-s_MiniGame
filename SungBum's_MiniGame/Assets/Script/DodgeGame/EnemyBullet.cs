using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int AttackPower = 0;

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().HP -= AttackPower;
            Destroy(this.gameObject);
        }

        else if (other.gameObject.CompareTag("Border"))
        {
            Destroy(this.gameObject);
        }

    }
}
