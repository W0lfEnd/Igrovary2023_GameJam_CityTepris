using Enemies;
using Game.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Game.Scripts;
using Game.Scripts.GameBoardLogic.Board;
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

    [SerializeField] private RectTransform gameoverPrefab        = null;
    [SerializeField] private RectTransform gameoverParent        = null;
    [SerializeField] private UIUpgradePanel upgradePanel = null;
    private int enemiesDied;
    private float startedPlayingAt;
    private bool hasEndedGame = false;

    void Awake()
    {
       enemiesDied = 0;
       startedPlayingAt = Time.time;
        InitializeSingleton();
        InitializeDimensionMusicTheme();

        onLvlChanged += newLvl => upgradePanel.tryToOpenPanel();
        Board.OnBoardChanged += ( firstList, secondList ) =>
        {
           if ( this == null || !this )
              return;

           cityBlockCount = firstList.Count + secondList.Count;
        };

    }

    private float          timer_to_give_xp = 0f;
    public  int            _cityBlockCount  = 0;
    
    public  event Action<int> cityBlocksCountChanged  = delegate {  };
    public int cityBlockCount
    {
       get => _cityBlockCount;
       private set
       {
          _cityBlockCount = value;
          cityBlocksCountChanged( _cityBlockCount );
       }
    }

    public int   xpPerCityBlockPerSecond = 1;

    private void Update()
    {
       timer_to_give_xp += Time.deltaTime;
       if ( timer_to_give_xp > 1f )
       {
          timer_to_give_xp = 0;
          xp += cityBlockCount * xpPerCityBlockPerSecond;
       }
    }

    private void ShowEndgamePanel()
    {
       hasEndedGame = true;
       var gameover = Instantiate(gameoverPrefab,gameoverParent);

       Time.timeScale = 0;
       FindObjectOfType<ShapeSpawner>().enabled = false;
    }
    public void OnEnemyDied()
    {
       enemiesDied++;
    }

    public int GetEnemiesDied()
    {
       return enemiesDied;
    }

    public float GetPlayTime()
    {
       return Time.time - startedPlayingAt;
    }
    
    public bool IsUpgradesPanelActive()
    {
       return upgradePanel.isActiveAndEnabled;
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
      gold = 0;
      xp = 0;
      health = maxHealth;
      
      upgradePanel.gameObject.SetActive( false );
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
         }
            
         
         onXpChanged( _xp );
      }
   }
   private int _xp = 0;

   public int lvl => xpToLvl( xp );

   public int xpToLvl( int amount )
   {
      return amount / 1000;
   }

   public int lvlToXp( int lvl )
   {
      return lvl * 1000;
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

         if (health <= 0 && !hasEndedGame)
         {
            ShowEndgamePanel();
         }
      }
   }
   private int _health = 0;

   public int maxHealth = 1000;
   #endregion

   #region Turrets
   [SerializeField] public List<TurretsController> TurretControllers = null;
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
