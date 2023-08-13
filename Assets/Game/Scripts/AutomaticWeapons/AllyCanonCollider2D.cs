using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AllyCanonCollider2D : MonoBehaviour
{
    public event Action<Collider2D>  onTriggerEnter2D   = delegate {};
    public event Action<Collider2D>  onTriggerStay2D    = delegate {};
    public event Action<Collision2D> onCollisionEnter2D = delegate {};
    public event Action<Collision2D> onCollisionStay2D  = delegate {};


    private void OnTriggerEnter2D( Collider2D col )
    {
        onTriggerEnter2D( col );
    }

    private void OnTriggerStay2D( Collider2D col )
    {
        onTriggerStay2D( col );
    }

    private void OnCollisionEnter2D( Collision2D col )
    {
        onCollisionEnter2D( col );
    }

    private void OnCollisionStay2D( Collision2D col )
    {
        onCollisionStay2D( col );
    }
}
