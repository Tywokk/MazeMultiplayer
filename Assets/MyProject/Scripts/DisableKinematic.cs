using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableKinematic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
