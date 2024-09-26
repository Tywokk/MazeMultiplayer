using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Fire : NetworkBehaviour
{
    [SerializeField] private Transform _gun;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private List<GameObject> spawnedBullets = new List<GameObject>();
    public float cooldownTime = 1f;
    private float nextFireTime = 0f;

    void Update()
    {
        if (IsOwner && Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            Debug.Log(IsOwnedByServer);
            ShootServerRpc(IsOwnedByServer, Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            nextFireTime = Time.time + cooldownTime;
        }
    }

    [ServerRpc]
    private void ShootServerRpc(bool host, float dirH, float dirV)
    {
        GameObject bullet;
        Vector3 offset = Vector3.zero;
        if (host == true)
        {
            bullet = Instantiate(_bulletPrefab, _gun.position, _gun.rotation);
        }
        else
        {
            Debug.Log("Aboba!!!");

                if (dirV < 0)
                {
                    offset += _gun.up * -1;
                }
                else if (dirV > 0)
                {
                    offset += _gun.up * 1;
                }
           
            bullet = Instantiate(_bulletPrefab, _gun.position + offset, _gun.rotation);
        }
        spawnedBullets.Add(bullet);
        bullet.GetComponent<Bullet>().parent = this;
        bullet.GetComponent<NetworkObject>().Spawn(true);
    }
    [ServerRpc(RequireOwnership = false)]
    public void DestroyServerRpc()
    {
        GameObject toDestroy = spawnedBullets[0];
        toDestroy.GetComponent<NetworkObject>().Despawn();
        spawnedBullets.Remove(toDestroy);
        Destroy(toDestroy);
    }
}