using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using UnityEngine;
using UnityEngine.Pool;


public class AllyCanon : MonoBehaviour
{
    [SerializeField] private AllyCanonProjectile go_projectile   = null;
    [SerializeField] private float                _Damage          = 10f;
    [SerializeField] private float                _AttackEveryMs   = 1500f;
    [SerializeField] private float                _ProjectileSpeed = 3f;
    [SerializeField] private float                _AttackRange     = 4f;
    [SerializeField] private float                _ExplosionRange  = 4f;

    public float AttacksPerSecond
    {
        get => 1f / _AttackEveryMs / 1000f;
        set
        {
            if ( value == 0 )
                value = 999999999999999;
            _AttackEveryMs = 1f / value * 1000f;
        }
    }
    
    public float AttackRange
    {
        get => _AttackRange;
        set
        {
            _AttackRange = value;
        }
    }

    public float ExplosionRange
    {
        get => _ExplosionRange;
        set
        {
            _ExplosionRange = value;
        }
    }

    private float                           TimeToNextShot  = 0.0f;
    private ObjectPool<AllyCanonProjectile> projectilesPool = null;

    private void Awake()
    {
        projectilesPool = new ObjectPool<AllyCanonProjectile>(
            () =>
            {
                var obj = Instantiate( go_projectile.gameObject, transform.position, Quaternion.identity ).GetComponent<AllyCanonProjectile>();
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
        TimeToNextShot += Time.deltaTime * 1000;
        if ( TimeToNextShot > _AttackEveryMs )
        {
            TimeToNextShot = 0;
            List<BaseEnemy> enemiesInRange = Physics2D.OverlapCircleAll( transform.position, AttackRange ).Select( it => it.gameObject.GetComponent<BaseEnemy>() ).Where( it => it != null ).ToList();
            if ( enemiesInRange.Count > 0 )
            {
                BaseEnemy closestEnemy = enemiesInRange.Aggregate( ( cur, min ) =>
                {
                    float distToCur = Vector2.Distance( cur.transform.position, transform.position );
                    float distToMin =  Vector2.Distance( min.transform.position, transform.position );
                    return distToCur < distToMin ? cur : min;
                } );

                shotTo( closestEnemy );
            }

        }
    }

    private void shotTo( BaseEnemy target )
    {
        var bullet = projectilesPool.Get();
        bullet.transform.position = transform.position;
        bullet.init( target, _Damage, _ProjectileSpeed, _ExplosionRange, () => projectilesPool.Release( bullet ) );

        SoundsManager.Instance.TryPlaySoundByType(SoundType.CannonShot);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere( transform.position, AttackRange );
        // Gizmos.color = Color.blue;
        // Gizmos.DrawSphere( transform.position, TimeToNextShot / AttackEveryMs );
    }
}
