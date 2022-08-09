using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Api : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(WWWGETTest());
    }

    IEnumerator WWWGETTest()
    {
        // GET 방식
        string apikey = "haMIFWTMA2Bq8rdxWFpRV-yxY40mgYdfqG7_btBeevw";
        string url = "https://mathpid.com/POST/api/v1/game/diagnosis/setting(D32546876549)?apikey=haMIFWTMA2Bq8rdxWFpRV-yxY40mgYdfqG7_btBeevw";

        UnityWebRequest www = UnityWebRequest.Get(url); // 동작은 하지만 현재는 사용하지 않은 추세이므로, UnityWebRequest를 사용해야 함.

        yield return www.SendWebRequest();

        if (www.error == null)
        {
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            Debug.Log("error");
        }
    }


}
