using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] float bulletPower = 10;
    [SerializeField] float bulletSpeed = 50;
    Rigidbody bulletRb;
   
    void Start()
    {
        bulletRb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            Rigidbody zombieRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 push = (collision.gameObject.transform.position - transform.position);
            zombieRb.AddForce(push * bulletPower, ForceMode.Impulse);
            Debug.Log("Bullet Hit!");
            Destroy(gameObject);
        }

        if(collision.gameObject.CompareTag("floor"))
        {
            Destroy(gameObject);
        }
    }
}
