using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaBoost : MonoBehaviour
{
    [SerializeField] int energy;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<FPSController>().ChangeMaxEnergy(energy);
            //gameObject.GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);
        }
    }
}
