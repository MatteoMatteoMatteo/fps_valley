using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private bool _chasing = false;
    public float distanceToChase = 20, distanceToLose = 25, distanceToStop = 2;
    private Vector3 _targetPoint, _startPoint;
    public NavMeshAgent agent;
    public GameObject bullet;
    public Transform firepoint;

    public float fireRate;
    private float _fireCount;
    private void Start()
    {
        _startPoint = transform.position;
    }

    private void Update()
    {

        _targetPoint = PlayerMovement.instance.transform.position;

        if (!_chasing)
        {
            if (Vector3.Distance(transform.position, _targetPoint) < distanceToChase)
            {
                _chasing = true;
                _fireCount = 1.5f;
            }

        }
        else
        {
            if (Vector3.Distance(transform.position, _targetPoint) > distanceToStop)
            {
                agent.destination = _targetPoint;
            }else
            {
                agent.destination = transform.position;
            }
            
            if (Vector3.Distance(transform.position, _targetPoint) > distanceToLose)
            {
                _chasing = false;
                agent.destination = _startPoint;
            }

            _fireCount -= Time.deltaTime;

            if (_fireCount <= 0)
            {
                _fireCount = fireRate;
                Instantiate(bullet, firepoint.position, firepoint.rotation);
            }
        }
    }
}
