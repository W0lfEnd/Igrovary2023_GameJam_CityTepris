using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using Enemies;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class AllyCanonProjectile : MonoBehaviour
{
    private float deltaDistanceToApplyDmg = 0.15f;

    [SerializeField] private GameObject          go_explosion_collider      = null;
    [SerializeField] private SpriteRenderer      projectile_sprite_renderer = null;
    [SerializeField] private AllyCanonCollider2D collider_events_holder     = null;

    private BaseEnemy target      = null;
    private Action    onCollision = null;
    private float     damage      = 1f;
    private float     speed       = 1f;
    private float     explosionScale = 1.5f;
    private float     explosionDuration = 0.3f;

    private Tweener tweener_explosion = null;


    private void Awake()
    {
        collider_events_holder.onTriggerEnter2D += col => col.gameObject.GetComponent<BaseEnemy>()?.Damage( (int)damage );
    }

    public void init( BaseEnemy target, float damage, float speed, float explosionRange, Action onCollision = null )
    {
        this.target         = target;
        this.onCollision    = onCollision;
        this.damage         = damage;
        this.speed          = speed;
        this.explosionScale = explosionRange;

        this.enabled = true;
        projectile_sprite_renderer.enabled = true;
        tweener_explosion = null;
    }

    private void Update()
    {
        if ( target == null )
        {
            enabled = false;
            onCollision?.Invoke();
            return;
        }

        if ( deltaDistanceToApplyDmg < Vector2.Distance( transform.position, target.transform.position ) )
        {
            //transform.up = target.transform.position - transform.position;
            transform.Translate( (target.transform.position - transform.position).normalized * (speed * Time.deltaTime), Space.World );
            return;
        }

        projectile_sprite_renderer.enabled = false;

        go_explosion_collider.SetActive( true );
        go_explosion_collider.transform.localScale = Vector3.zero;
        tweener_explosion = go_explosion_collider
                            .transform
                            .DOScale( explosionScale * Vector3.one, explosionDuration )
                            .Play();

        tweener_explosion.onComplete += () =>
        {
            go_explosion_collider.SetActive( false );
            onCollision?.Invoke();
        };

        enabled = false;
    }

    private void OnDisable()
    {
        go_explosion_collider.transform.localScale = Vector3.one;
        DOTween.Kill( tweener_explosion );
    }
    }
