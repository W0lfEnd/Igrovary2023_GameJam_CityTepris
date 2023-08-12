using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;


public class AllyTurretProjectile : MonoBehaviour
{
    private Transform target      = null;
    private Action    onCollision = null;
    private float     damage      = 1f;
    private float     speed       = 1f;


    public void init( Transform target, float damage, float speed, Action onCollision = null )
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
            onCollision?.Invoke();
            enabled = false;
        }

        transform.Translate( (target.position - transform.position).normalized * (speed * Time.deltaTime) );
    }

    private void OnCollisionEnter2D( Collision2D col )
    {
        Object targetComponent = null; //col.gameObject.GetComponent<>();
        if ( targetComponent != null )
        {
            //do damage
            onCollision();
        }
    }
}
