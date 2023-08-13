using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIBtnSelectUpgrade : MonoBehaviour
{
    public event Action<UIUpgradePanel.UpgradeData> onClicked = delegate {};


    [SerializeField] private Image           image           = null;
    [SerializeField] private TextMeshProUGUI txt_title       = null;
    [SerializeField] private TextMeshProUGUI txt_description = null;


    private Button                     button        = null;
    private Action                     onClickAction = null;
    private UIUpgradePanel.UpgradeData upgrade_data  = null;


    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener( onClick );
    }

    public void init( UIUpgradePanel.UpgradeData upgrade_data )
    {
        this.upgrade_data = upgrade_data;

        image.sprite = upgrade_data.sprite;
        txt_title.text = upgrade_data.title;
        txt_description.text = upgrade_data.description?.Invoke() ?? "";
        onClickAction = upgrade_data.onClickAction;
    }

    private void onClick()
    {
        onClickAction?.Invoke();
        onClicked(upgrade_data);
    }
}
