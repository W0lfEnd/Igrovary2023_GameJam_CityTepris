using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   #region Singleton Logic
   public static GameManager Instance { get; private set; }


   void Awake()
   {
      if ( Instance == null )
      {
         Instance = this;
      }
      else
      if ( Instance == this )
      {
         Destroy( gameObject );
      }
   }

   private void Start()
   {
      init();
   }
   #endregion

   #region Events
   public event Action<int> onGoldChanged = delegate {}; 
   #endregion

   private void init()
   {
      turretsLvl = 0;
      gold = 0;
   }

   #region Economics
   public int gold
   {
      get => _gold;
      set
      {
         _gold = value;
         onGoldChanged( _gold );
      }
   }
   public int _gold = 0;
   #endregion

   #region Turrets
   public int turretsLvl
   {
      get => _turretLvl;
      set
      {
         _turretLvl = value;
         setTurretsLvl( _turretLvl );
      }
   }

   public int _turretLvl = 0;

   [SerializeField] private List<TurretsController> TurretControllers = null;

   public void setTurretsLvl( int lvl )
   {
      foreach ( TurretsController turretController in TurretControllers )
      {
         turretController.init( lvl );
      }
   }

   public int goldCostForTurretsUpgrade()
   {
      return needGoldForUpgradeToLvl( turretsLvl + 1 );
   }

   public int needGoldForUpgradeToLvl( int lvl )
   {
      switch ( lvl )
      {
         case 0: return 0;
         case 1: return 100;
         case 2: return 200;
         case 3: return 300;
         case 4: return 400;
         
         default: return 999999999;
      }
   }

   public bool tryUpgradeTurrets()
   {
      int upgradeCost = goldCostForTurretsUpgrade();
      if ( gold >= upgradeCost )
      {
         gold -= upgradeCost;
         turretsLvl++;
         return true;
      }

      return false;
   }

   public void CHEAT_upgradeTurretsLvl() => turretsLvl++;
   #endregion
}
