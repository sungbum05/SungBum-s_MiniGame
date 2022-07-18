using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int MaxHp = 100;

    [SerializeField] private int hp; // 프로퍼티지만 확인하기 위해
    public int HP
    {
        get
        {
            return hp;
        }

        set
        {
            hp = value;

            if (hp <= 0)
                Debug.Log("Die");
        }
    }

    [SerializeField] private Vector2 BorderMaxPos;
    [SerializeField] private Vector2 BorderMinPos;

    [SerializeField] private float Speed;

    // Start is called before the first frame update
    void Start()
    {
        PlayerSetting();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
    }

    void PlayerSetting()
    {
        HP = MaxHp;
    }

    void PlayerMove()
    {

        //#region GetAxis
        //float x = Input.GetAxis("Horizontal");
        //float y = Input.GetAxis("Vertical");
        //#endregion 
        //-1f ~ 1f

        #region GetAxisRaw
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        #endregion 
        // -1.0f, 0, 1.0f

        this.gameObject.transform.position += new Vector3(x, y, 0) * Time.deltaTime * Speed;

        this.gameObject.transform.position = new Vector3(Mathf.Clamp(this.gameObject.transform.position.x, BorderMinPos.x, BorderMaxPos.x),
            Mathf.Clamp(this.gameObject.transform.position.y, BorderMinPos.y, BorderMaxPos.y), 0); // 움직일 수 있는 영역 설정
    }
}
