using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ObjectVisibilityManager : NetworkBehaviour
{
    private static List<NetworkObject> _networkObjects;
    [SerializeField] private float _visibilityRadius;

    public static void AddNetworkObject(NetworkObject networkObject)
    {
        _networkObjects.Add(networkObject);
    }

    private void Start()
    {
        _networkObjects = new List<NetworkObject>();
        InvokeRepeating("ObjectVisibility", 0.5f, 0.5f);
    }

    void ObjectVisibility()
    {
        if (!IsHost) return;
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            if (client.Key == NetworkManager.LocalClientId) continue;

            foreach (var networkObject in _networkObjects)
            {
                var clientTransform = client.Value.PlayerObject.transform;
                bool isVisible = Vector3.Distance(clientTransform.position, networkObject.transform.position) < _visibilityRadius;

                if (networkObject.IsNetworkVisibleTo(client.Key) != isVisible)
                {
                    if (isVisible)
                    {
                        networkObject.NetworkShow(client.Key);
                    }
                    else
                    {
                        networkObject.NetworkHide(client.Key);
                    }
                }
            }
        }
    }
}