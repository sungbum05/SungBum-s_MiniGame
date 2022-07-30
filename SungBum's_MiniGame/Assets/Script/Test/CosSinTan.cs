using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosSinTan : MonoBehaviour
{
    [SerializeField] float sightDistance = 10;
    [SerializeField] float sightDegree = 30;

    Vector3[] CalualteSightPoint(float radius, float angle)
    {
        Vector3[] results = new Vector3[2];

        //우측 끝 점의 좌표를 구한다
        float theta = 90 - angle - transform.eulerAngles.y;
        float posX = Mathf.Cos(theta * Mathf.Deg2Rad) * radius;
        float posY = transform.position.y;
        float posZ = Mathf.Sin(theta * Mathf.Deg2Rad) * radius;
        results[0] = new Vector3(posX, posY, posZ);

        theta = 90 + angle - transform.eulerAngles.y;
        posX = Mathf.Cos(theta * Mathf.Deg2Rad) * radius;
        posY = transform.position.y;
        posZ = Mathf.Sin(theta * Mathf.Deg2Rad) * radius;
        results[1] = new Vector3(posX, posY, posZ);

        return results;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Vector3[] sightPos = CalualteSightPoint(sightDistance, sightDegree);

        for (int i = 0; i < sightPos.Length; i++)
        {
            Gizmos.DrawLine(transform.position, transform.position + sightPos[i]);
        }
    }
}
