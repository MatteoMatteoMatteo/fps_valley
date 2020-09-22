using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float moveSpeed, lifeTime;
    public Rigidbody theRB;
    public int power;
    public GameObject gravityGunImpactEffect;
    public GameObject enemyImpactEffect;


    // Update is called once per frame
    void Update()
    {
        theRB.velocity = transform.forward * moveSpeed;

        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        var rigid = other.transform.gameObject.GetComponent<Rigidbody>();
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Instantiate(enemyImpactEffect, transform.position + (transform.forward * (-moveSpeed * Time.deltaTime)), transform.rotation);
        }
        else if (rigid)
        {
            Destroy(gameObject);
            rigid.AddForce(transform.forward * power);
            Instantiate(gravityGunImpactEffect, transform.position + (transform.forward * (-moveSpeed * Time.deltaTime)), transform.rotation);
        }




    }
}
