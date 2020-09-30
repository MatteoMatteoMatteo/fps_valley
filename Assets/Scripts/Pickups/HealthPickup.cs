using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private bool collected=false;
    public int healAmount;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !collected)
        {
            PlayerHealthController.instance.HealPlayer(healAmount);
            Destroy(gameObject);
            collected = true;
        }
    }
}
