using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class UILookAtCamera : NetworkBehaviour
{
    private List<GameObject> players;
    GameObject closest;
    public string nearest;
    public void FindPlayers()
    {
        if (!IsOwner) return;
        Debug.Log("Proverka");
        players = GameObject.FindGameObjectsWithTag("Player").ToList();
        if (players.Count > 1) 
        {
            int c = 0;
            int ind = 0;
            foreach (GameObject p in players)
            {
                if (p.GetComponent<NetworkObject>().OwnerClientId == OwnerClientId)
                {
                    ind = c;
                }
                c++;
            }
            players.RemoveAt(ind);
        }
    }
    public void OnEnable()
    {
        FPSController.onPlayerSpawn += FindPlayers;
    }
    public void OnDisable()
    {
        FPSController.onPlayerSpawn -= FindPlayers;
    }
    GameObject FindClosestPlayer()
    {
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in players)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
    private void FixedUpdate()
    {
        if (!IsOwner) return;
        transform.LookAt(FindClosestPlayer().transform.position);
    }
}