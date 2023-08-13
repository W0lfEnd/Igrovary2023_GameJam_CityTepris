using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretsController : MonoBehaviour
{
  [SerializeField] public AllyTurret     turret      = null;
  [SerializeField] public AllyBladeStorm blade_storm = null;
  [SerializeField] public AllyCanon      canon       = null;


  private void Awake()
  {
    blade_storm.init();

    turret     .gameObject.SetActive( true  );
    blade_storm.DisplayBlades( false );
    canon      .gameObject.SetActive( false );
  }
}
