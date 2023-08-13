using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;


public class AllyBladeStorm_Blade : MonoBehaviour
{
  [SerializeField] private int damage = 10;

  public void init( int damage )
  {
    this.damage = damage;
  }

  private void OnTriggerEnter2D( Collider2D col )
  {
    BaseEnemy enemy = col.gameObject.GetComponent<BaseEnemy>();
    if ( enemy )
    {
      enemy.Damage( damage );
    }
  }
}
