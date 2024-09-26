using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : NetworkBehaviour
{
    [SerializeField] Slider slider;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            gameObject.SetActive(false);
        }
    }
    public void SetMaxStamina(float energy)
    {
        if (!IsOwner) return;
        slider.maxValue = energy;
        slider.value = energy;
    }
    public void ChangeMaxStamina(float energy)
    {
        if (!IsOwner) return;
        slider.maxValue += energy;
    }
    public void SetStamina(float energy)
    {
        Debug.Log("Set Energy before");
        if (!IsOwner) return;
        Debug.Log("Set Energy after");
        slider.value = energy;
    }
}

