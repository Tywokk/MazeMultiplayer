using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : NetworkBehaviour
{
    [SerializeField] Slider slider;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            gameObject.SetActive(false);
        }
    }
    public void SetMaxHealth(int health)
    {
        if (!IsOwner) return;
        slider.maxValue = health;
        slider.value = health;
    }
    public void ChangeMaxHealth(int health)
    {
        if (!IsOwner) return;
        slider.maxValue += health;
    }
    public void SetHealth(int health)
    {
        Debug.Log("Set Health before");
        if (!IsOwner) return;
        Debug.Log("Set Health after");
        slider.value = health;
    }
}
