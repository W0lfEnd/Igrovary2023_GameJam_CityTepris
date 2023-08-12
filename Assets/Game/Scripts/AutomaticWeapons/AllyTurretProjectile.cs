using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;


public class AllyTurretProjectile : MonoBehaviour
{
    private float deltaDistanceToApplyDmg = 0.1f;

    private BaseEnemy target      = null;
    private Action    onCollision = null;
    private float     damage      = 1f;
    private float     speed       = 1f;


    public void init( BaseEnemy target, float damage, float speed, Action onCollision = null )
    {
        this.target      = target;
        this.onCollision = onCollision;
        this.damage      = damage;
        this.speed       = speed;
        this.enabled     = true;
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
            target.Damage( (int)damage );
            onCollision?.Invoke();
        }
    }
}
