using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConroller : MonoBehaviour
{
    private bool _chasing = false;
    public float distanceToChase = 20;
    public float distanceToLose = 25;
    public float moveSpeed;
    public Rigidbody theRb;
    private Vector3 _targetPoint;

    private void Update()
    {

        _targetPoint = PlayerMovement.instance.transform.position;
        _targetPoint.y = transform.position.y;
        
        if (!_chasing)
        {
            if (Vector3.Distance(transform.position, _targetPoint) < distanceToChase)
            {
                _chasing = true;
            }
        }
        else
        {
            transform.LookAt(_targetPoint);
            theRb.velocity = transform.forward * moveSpeed;
            
            if (Vector3.Distance(transform.position, _targetPoint) > distanceToLose)
            {
                _chasing = false;
            } 
        }
    }
}
