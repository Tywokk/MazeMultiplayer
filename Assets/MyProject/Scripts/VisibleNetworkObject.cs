using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class VisibleNetworkObject : MonoBehaviour
{
    void Start()
    {
        ObjectVisibilityManager.AddNetworkObject(GetComponent<NetworkObject>());
    }
}
