using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth;
    public HealthBar healthBar;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }
    private void OnEnable()
    {
        //Bullet.onHitted += TakeDamage;
    }
    private void OnDisable()
    {
        //Bullet.onHitted -= TakeDamage;
    }
    //id != gameObject.GetComponent<NetworkObject>().OwnerClientId
    public void TakeDamage(int damage, ulong id)
    {
        Debug.Log("Take damage");
        if (id != OwnerClientId)
        {
            DamageClientRpc(damage);
        }
    }
    public bool Heal(int heal)
    {
        Debug.Log("Heal");
        if (currentHealth < maxHealth)
        {
            HealClientRpc(heal);
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ChangeMaxHealth(int health)
    {
        Debug.Log("Set max health");
        if (true)
        {
            ChangeMaxHealthClientRpc(health);
        }
    }
    [ClientRpc]
    public void DamageClientRpc(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0) 
        {
            Destroy(gameObject);
        }
    }
    [ClientRpc]
    public void HealClientRpc(int heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.SetHealth(currentHealth);
    }
    [ClientRpc]
    public void ChangeMaxHealthClientRpc(int health)
    {
        maxHealth += health;
        healthBar.ChangeMaxHealth(health);
    }
}
