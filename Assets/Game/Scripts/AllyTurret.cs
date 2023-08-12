using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyTurret : MonoBehaviour
{
    [SerializeField] private GameObject go_projectile   = null;
    [SerializeField] private float      Damage          = 10f;
    [SerializeField] private float      AttackEveryMs   = 300f;
    [SerializeField] private float      ProjectileSpeed = 10f;
    [SerializeField] private float      AttackRange     = 10f;

    private float TimeToNextShot = 0.0f;


    private void Update()
    {
        
    }

    private void spawnProjectile( Transform target )
    {
        Instantiate( go_projectile, transform );
    }
}
