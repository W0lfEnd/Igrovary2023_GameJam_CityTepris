using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using UnityEngine;
using UnityEngine.Pool;


public class AllyTurret : MonoBehaviour
{
    [SerializeField] private AllyTurretProjectile go_projectile   = null;
    [SerializeField] private LineRenderer         line_renderer_radius   = null;
    [SerializeField] private float                _Damage          = 10f;
    [SerializeField] private float                _AttackEveryMs   = 250f;
    [SerializeField] private float                _ProjectileSpeed = 10f;
    [SerializeField] private float                _AttackRange     = 2.5f;

    public float AttacksPerSecond
    {
        get => 1000f / _AttackEveryMs;
        set
        {
            if ( value == 0 )
                value = 999999999999999;
            _AttackEveryMs = 1000f / value;
        }
    }
    
    public float AttackRange
    {
        get => _AttackRange;
        set
        {
            _AttackRange = value;
            setLineRendererRange ();
        }
    }

    private float                            TimeToNextShot  = 0.0f;
    private ObjectPool<AllyTurretProjectile> projectilesPool = null;

    private void Awake()
    {
        line_renderer_radius.SetVertexCount (50 + 1);
        line_renderer_radius.useWorldSpace = false;
        setLineRendererRange();

        projectilesPool = new ObjectPool<AllyTurretProjectile>(
            () =>
            {
                var obj = Instantiate( go_projectile.gameObject, transform.position, Quaternion.identity ).GetComponent<AllyTurretProjectile>();
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
        bullet.init( target, _Damage, _ProjectileSpeed, () => projectilesPool.Release( bullet ) );

        SoundsManager.Instance.TryPlaySoundByType(SoundType.TurretShoot);
        SoundsManager.Instance.TryPlaySoundByType(SoundType.TurretReloading);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere( transform.position, AttackRange );
        // Gizmos.color = Color.blue;
        // Gizmos.DrawSphere( transform.position, TimeToNextShot / AttackEveryMs );
    }

    void setLineRendererRange ()
    {
        float x;
        float y;
        float z;

        float angle = 20f;

        for (int i = 0; i < (50 + 1); i++)
        {
            x = Mathf.Sin (Mathf.Deg2Rad * angle) * AttackRange;
            y = Mathf.Cos (Mathf.Deg2Rad * angle) * AttackRange;

            line_renderer_radius.SetPosition (i, new Vector3(x, y, 0) );

            angle += (360f / 50);
        }
    }
}
