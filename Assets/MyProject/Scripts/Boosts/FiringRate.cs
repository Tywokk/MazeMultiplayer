using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FiringRate : NetworkBehaviour
{
    [SerializeField] float rate;
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<Fire>().cooldownTime -= rate;
            //gameObject.GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);
        }
    }
}
