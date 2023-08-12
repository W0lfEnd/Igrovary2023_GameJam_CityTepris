using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretsController : MonoBehaviour
{
  [SerializeField] private AllyTurret turret = null;
  [SerializeField] private AllyBladeStorm blade_storm = null;


  private void Awake()
  {
    blade_storm.init();
  }

  public void init( int lvl = 0 )
  {
    turret.gameObject.SetActive( true );
    blade_storm.gameObject.SetActive( lvl > 1 );
  }
  
}
