using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.UI;
using UnityEngine;

public class UITopPanel : MonoBehaviour
{
    [SerializeField] private UiBar barHealth = null;
    [SerializeField] private UiBar barXp     = null;


    private static GameManager gm => GameManager.Instance;

    private void Awake()
    {
        gm.onLvlChanged += new_lvl => initXpBar();
        gm.onXpChanged += new_lvl => setValXpBar();

        barHealth.Initialize( gm.maxHealth );
        gm.onHealthChanged += health => barHealth.SetValue( health );;
    }

    // Start is called before the first frame update
    void Start()
    {
        initXpBar();
    }

    private void initXpBar()
    {
        barXp.Initialize(
            gm.xpToLvl( gm.lvl + 1 ) - gm.xpToLvl( gm.lvl )
          , new_xp_amount => gm.xpToLvl( new_xp_amount ).ToString()
        );
    }

    private void setValXpBar()
    {
        barXp.SetValue( gm.xp - gm.xpToLvl( gm.lvl ) );
    }
}
