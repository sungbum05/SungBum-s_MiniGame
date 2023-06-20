using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeServerManager : MonoBehaviour
{
    public static TypeServerManager Instance;
    public void Awake() => Instance = this;

    public TypeClient TypeClient;
    public TypeServer TypeServer;
    public TypeChat TypeChat;

    public MultiInput MultiInput;
}
