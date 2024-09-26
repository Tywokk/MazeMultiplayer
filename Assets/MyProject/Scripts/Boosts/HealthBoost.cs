using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoost : MonoBehaviour
{
    [SerializeField] int health;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<PlayerHealth>().ChangeMaxHealth(health);
            //gameObject.GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);
        }
    }
}
