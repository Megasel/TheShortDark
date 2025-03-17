using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float bulletSpeed;
    public void Shot()
    {
        //rb.AddForce(Vector3.forward * bulletSpeed);
    }
}
