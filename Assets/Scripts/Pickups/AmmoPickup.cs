using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    private bool collected=false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !collected)
        {
            PlayerController.instance.activeGun.GetAmmo();
            Destroy(gameObject);
            collected = true;
        }
    }
}
