using Unity.Netcode;
using UnityEngine;
using System;
using System.Collections.Generic;

public class Cell : NetworkBehaviour
{
    public GameObject WallLeft;
    public GameObject WallBottom;
    public List<GameObject> Boosts = new List<GameObject>();
    public MazeSpawner parent;
    [SerializeField] NetworkVariable<bool> wallLeftDeactivate = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] NetworkVariable<bool> wallBottomDeactivate = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] NetworkVariable<int> boostActivateNum = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    float _timer = 0f;
    float _lifeTime = 0.01f;
    public override void OnNetworkSpawn()
    {
        Debug.Log("Cell spawned");
        if (IsOwnedByServer)
        {
            Debug.Log("Is server");
            DeactivateWallLeftServerRpc();
            DeactivateWallBottomServerRpc();
            ActivateBoostServerRpc();
        }
        WallLeft.GetComponent<MeshRenderer>().enabled = !wallLeftDeactivate.Value;
        WallLeft.GetComponent<Collider>().enabled = !wallLeftDeactivate.Value;
        WallBottom.GetComponent<MeshRenderer>().enabled = !wallBottomDeactivate.Value;
        WallBottom.GetComponent<Collider>().enabled = !wallBottomDeactivate.Value;
        if (Boosts.Count != 0)
        {
            Boosts[boostActivateNum.Value].SetActive(true);
        }
    }
    private void Update()
    {

    }
    [ServerRpc(RequireOwnership = false)]
    public void DeactivateWallLeftServerRpc()
    {
        if (WallLeft.GetComponent<MeshRenderer>().enabled == false)
        {
            wallLeftDeactivate.Value = true;
            Debug.Log("Deactivate left wall");
        }
        else
        {
            wallLeftDeactivate.Value = false;
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void DeactivateWallBottomServerRpc()
    {
        if (WallBottom.GetComponent<MeshRenderer>().enabled == false)
        {
            wallBottomDeactivate.Value = true;
            Debug.Log("Deactivate bottom wall");
        }
        else
        {
            wallBottomDeactivate.Value = false;
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void ActivateBoostServerRpc()
    {
        if (Boosts.Count != 0)
        {
            int count = 0;
            foreach (var boost in Boosts)
            {
                if (boost.activeSelf == true)
                {
                    boostActivateNum.Value = count;
                }
                count++;
            }
        }
    }
}