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
        gm.onHealthChanged += health => barHealth.SetValue( health );
    }

    // Start is called before the first frame update
    void Start()
    {
        initXpBar();
        setValXpBar();
    }

    private void initXpBar()
    {
        barXp.Initialize(
            gm.lvlToXp( gm.lvl + 1 ) - gm.lvlToXp( gm.lvl )
          , new_xp_amount => (gm.lvl + 1).ToString()
        );
    }

    private void setValXpBar()
    {
        barXp.SetValue( gm.xp - gm.lvlToXp( gm.lvl ) );
    }
}
