using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Api : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetApi());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GetApi()
    {
        string url = "POST/api/v1/game/diagnosis/setting";
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.error == null)
        {
            Debug.Log(www.downloadHandler.text);
        }

        else
            Debug.Log("ERROR");
    }
}
