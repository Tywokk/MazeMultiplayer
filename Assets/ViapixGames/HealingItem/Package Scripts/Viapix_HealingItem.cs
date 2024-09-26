using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viapix_PlayerParams;
using Unity.Netcode;
namespace Viapix_HealingItem
{
    public class Viapix_HealingItem : NetworkBehaviour
    {
        [SerializeField]
        float rotationSpeedX, rotationSpeedY, rotationSpeedZ;

        [SerializeField]
        int healingAmount;

        void Update()
        {
            transform.Rotate(rotationSpeedX, rotationSpeedY, rotationSpeedZ);
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                bool IsHeal = collider.gameObject.GetComponent<PlayerHealth>().Heal(healingAmount);
                //gameObject.GetComponent<NetworkObject>().Despawn();
                if (IsHeal)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}

