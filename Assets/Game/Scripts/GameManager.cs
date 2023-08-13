using Enemies;
using Game.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Game.Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton Logic
    public static GameManager Instance { get; private set; }

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance == this)
            {
                Destroy(gameObject);
            }
        }
    }
    #endregion

    void Awake()
    {
        InitializeSingleton();
        InitializeDimensionMusicTheme();
    }

    private void Start()
    {
        init();
    }

   #region Events
   public event Action<int> onGoldChanged = delegate {}; 
   public event Action<int> onXpChanged = delegate {}; 
   public event Action<int> onLvlChanged = delegate {}; 
   public event Action<int> onHealthChanged = delegate {};
   #endregion

   private void init()
   {
      turretsLvl = 0;
      gold = 0;
      xp = 0;
      health = maxHealth;
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
   private int _gold = 0;
   
   public int xp
   {
      get => _xp;
      set
      {
         int old_val = _xp;
         _xp = value;

         int new_lvl = xpToLvl( _xp );
         int old_lvl = xpToLvl( old_val );
         if ( new_lvl > old_lvl )
         {
            onLvlChanged( new_lvl );
            turretsLvl += new_lvl - old_lvl;
         }
            
         
         onXpChanged( _xp );
      }
   }
   private int _xp = 0;

   public int lvl => xpToLvl( xp );

   public int xpToLvl( int amount )
   {
      if ( amount >= 1000 )
         return 4;

      if ( amount >= 500 )
         return 3;

      if ( amount >= 300 )
         return 2;
         
      if ( amount >= 100 )
         return 1;

      return 0;
   }

   public int lvlToXp( int lvl )
   {
      switch ( lvl )
      {
         case 0:  return 0;
         case 1:  return 100;
         case 2:  return 300;
         case 3:  return 500;
         case 4:  return 700;
         case 5:  return 1000;

         default: return 99999999;
      }
   }

   public int health
   {
      get => _health;
      set
      {
         _health = value;
         if ( _health < 0 )
            _health = 0;

         if ( health >= maxHealth )
            _health = maxHealth;

         onHealthChanged( _health );
      }
   }
   private int _health = 0;

   public int maxHealth = 100;
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

   private int _turretLvl = 0;

   [SerializeField] private List<TurretsController> TurretControllers = null;

   public void setTurretsLvl( int lvl )
   {
      foreach ( TurretsController turretController in TurretControllers )
      {
         turretController.init( lvl );
      }
   }

   public void CHEAT_upgradeTurretsLvl() => turretsLvl++;
   #endregion
   
   #region WorldSwap

    [SerializeField] private WorldSwapper _worldSwapper;

    public Dimension Dimension => _worldSwapper.DimensionType;

    public void SwapWorld()
    {
        _worldSwapper.SwapWorld();
        InitializeDimensionMusicTheme();
    }

    #endregion

    #region DimensionMusicTheme

    [SerializeField] private AudioSource _topDimensionMusicTheme;
    [SerializeField] private AudioSource _bottomDimensionMusicTheme;

    private void InitializeDimensionMusicTheme()
    {
        bool isTopDimension = Dimension == Dimension.TopDimesion;
        _topDimensionMusicTheme.enabled = isTopDimension;
        _bottomDimensionMusicTheme.enabled = !isTopDimension;
    }

   #endregion
}
