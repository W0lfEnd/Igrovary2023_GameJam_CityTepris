using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class AllyTurret : MonoBehaviour
{
    [SerializeField] private GameObject go_projectile   = null;
    [SerializeField] private float      Damage          = 10f;
    [SerializeField] private float      AttackEveryMs   = 300f;
    [SerializeField] private float      ProjectileSpeed = 10f;
    [SerializeField] private float      AttackRange     = 10f;

    private float                            TimeToNextShot  = 0.0f;
    private ObjectPool<AllyTurretProjectile> projectilesPool = null;

    private void Awake()
    {
        projectilesPool = new ObjectPool<AllyTurretProjectile>(
            () =>
            {
                var obj = Instantiate( go_projectile, transform ).GetComponent<AllyTurretProjectile>();
                obj.gameObject.SetActive( false );
                return obj;
            }
          , obj => { obj.gameObject.SetActive( true ); }
          , obj => { obj.gameObject.SetActive( false ); }
        );
    }


    private void Update()
    {
        
    }

    private void spawnProjectile( Transform target )
    {
        var bullet = projectilesPool.Get();
        bullet.init( target, ProjectileSpeed, () => projectilesPool.Release( bullet ) );
    }
}
