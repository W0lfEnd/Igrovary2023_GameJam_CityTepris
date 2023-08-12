using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
          , obj => { obj.gameObject.SetActive( false ); },
            defaultCapacity: 100
        );
    }


    private void Update()
    {
        TimeToNextShot += Time.deltaTime;
        if ( TimeToNextShot > AttackEveryMs )
        {
            var enemiesInRange = Physics2D.OverlapCircleAll( transform.position, AttackRange );
            if ( enemiesInRange.Any() )
            {
                var closestEnemy = enemiesInRange.Aggregate( ( cur, min ) =>
                {
                    float distToCur = Vector2.Distance( cur.transform.position, transform.position );
                    float distToMin =  Vector2.Distance( min.transform.position, transform.position );
                    return distToCur < distToMin ? cur : min;
                } );

                spawnProjectile( closestEnemy.transform );
            }

        }
            
    }

    private void spawnProjectile( Transform target )
    {
        var bullet = projectilesPool.Get();
        bullet.init( target, Damage, ProjectileSpeed, () => projectilesPool.Release( bullet ) );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere( transform.position, AttackRange );
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere( transform.position, TimeToNextShot / AttackEveryMs );
    }
}
