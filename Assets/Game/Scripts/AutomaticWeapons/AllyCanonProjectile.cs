using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;


public class AllyCanonProjectile : MonoBehaviour
{
    private float deltaDistanceToApplyDmg = 0.1f;

    private GameObject explosion      = null;

    private BaseEnemy target      = null;
    private Action    onCollision = null;
    private float     damage      = 1f;
    private float     speed       = 1f;
    private float     explosionRange = 0.3f;


    public void init( BaseEnemy target, float damage, float speed, float explosionRange, Action onCollision = null )
    {
        this.target         = target;
        this.onCollision    = onCollision;
        this.damage         = damage;
        this.speed          = speed;
        this.explosionRange = explosionRange;

        this.enabled = true;
    }

    private void Update()
    {
        if ( target == null )
        {
            enabled = false;
            onCollision?.Invoke();
        }

        transform.up = target.transform.position - transform.position;
        transform.Translate( (target.transform.position - transform.position).normalized * (speed * Time.deltaTime), Space.World );

        if ( deltaDistanceToApplyDmg > Vector2.Distance( transform.position, target.transform.position ) )
        {
            onCollision?.Invoke();
        }
    }

    private void OnCollisionEnter2D( Collision2D col )
    {
        
    }
}
