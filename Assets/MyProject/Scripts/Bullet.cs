using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System;

public class Bullet : NetworkBehaviour
{
    [SerializeField] float _force;
    [SerializeField] float _timer = 0f;
    [SerializeField] int _damage = 20;
    [SerializeField] float _lifeTime = 10f;
    public Fire parent;
    //public static Action<int, ulong> onHitted;
    public override void OnNetworkSpawn()
    {

    }
    private void Awake()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(transform.up * _force, ForceMode.Impulse);
    }
    private void Update()
    {
        if (!IsOwner) return;
        _timer += Time.deltaTime;
        if (_timer > _lifeTime)
        {
            parent.DestroyServerRpc();
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (!IsOwner) return;
        if (other.gameObject.CompareTag("Player"))
        {
            //onHitted?.Invoke(_damage, parent.gameObject.GetComponent<NetworkObject>().OwnerClientId);
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(_damage, parent.gameObject.GetComponent<NetworkObject>().OwnerClientId);
        }
    }

    
}
