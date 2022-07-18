using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image HpBarImage;
    [SerializeField] Player Player;
    [SerializeField] Text ScoreTxt;

    [SerializeField] DodgeGameManager DodgeGameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HpBarImage.fillAmount = (float)Player.HP / (float)Player.MaxHp;

        ScoreTxt.text = $"Score : {(int)DodgeGameManager.Score}";
    }
}
