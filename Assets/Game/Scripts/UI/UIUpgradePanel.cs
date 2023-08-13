using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;


public class UIUpgradePanel : MonoBehaviour
{
    [SerializeField] private UIBtnSelectUpgrade btnFirst = null;
    [SerializeField] private UIBtnSelectUpgrade btnSecond = null;

    private static GameManager gm => GameManager.Instance;
    private static TurretsController turretsController => GameManager.Instance.TurretControllers.FirstOrDefault();

    private        HashSet<int> alreadyUpgraded = new HashSet<int>();
    private static Random       random          = new Random();

    private void Awake()
    {
        btnFirst.onClicked += onAnyBtnClicked;
        btnSecond.onClicked += onAnyBtnClicked;
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    private void onAnyBtnClicked( UpgradeData upgrade_data )
    {
        alreadyUpgraded.Add( upgrade_data.id );
        closePanel();
    }

    public void tryToOpenPanel()
    {
        List<int> available_to_upgrade_list = mocks.Keys.Where( canUpgrade ).Where( it => !alreadyUpgraded.Contains( it ) ).ToList();
        if ( available_to_upgrade_list.Count == 0 )
            return;

        Shuffle( available_to_upgrade_list );
        gameObject.SetActive( true );
        btnFirst.init( mocks[available_to_upgrade_list[0]] );
        
        btnSecond.gameObject.SetActive( available_to_upgrade_list.Count > 1 );
        if ( available_to_upgrade_list.Count > 1 )
            btnSecond.init( mocks[available_to_upgrade_list[1]] );
    }

    public void closePanel() => gameObject.SetActive( false );

    public static void Shuffle<T>( IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k     = random.Next(n + 1);  
            T   value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }

    private bool canUpgrade( int id )
    {
        switch ( id )
        {
            case 0: return !turretsController.blade_storm.gameObject.activeSelf;
            case 1: return !turretsController.canon.gameObject.activeSelf;
            case 2: return turretsController.blade_storm.gameObject.activeSelf;

            case 5: return turretsController.canon.gameObject.activeSelf;
            case 6: return turretsController.canon.gameObject.activeSelf;
            case 7: return turretsController.canon.gameObject.activeSelf;
        }

        return true;
    }

    private Dictionary<int, UpgradeData> mocks = new  Dictionary<int, UpgradeData>()
    {
        [0] = new ()
        {
            id = 0,
            title = "Ріжучі леза лазерної заточки",
            description = () => "леза, що будуть крутитись покругу центрального будинку",
            onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.blade_storm.gameObject.SetActive( true ) )
        },
        [1] = new ()
        {
            id = 1,
            title = "Придбати потужну 'Пушку масового ураження'",
            description = () => "стріляє патронами, які взриваються",
            onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.canon.gameObject.SetActive( true ) )
        },
        [2] = new ()
        {
            id = 2,
            title = "Додаткове лезо (+1)",
            description = () => $"наявна кількість лез: {GameManager.Instance.TurretControllers[0].blade_storm.bladesCount}",
            onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.blade_storm.bladesCount++ )
        },
        [3] = new ()
        {
            id = 3,
            title = "Пришвидшити 'Дефолтну зброю'",
            description = () => "+1 в секунду",
            onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.turret.AttacksPerSecond++  )
        },
        [4] = new ()
        {
            id = 4,
            title = "Збільшити радіус 'Дефолтної зброї'",
            description = () => "+20%",
            onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.turret.AttackRange *= 1.2f  )
        },
        [5] = new ()
        {
            id = 5,
            title = "Пришвидшити 'Пушку масового ураження'",
            description = () => "+1 в секунду",
            onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.canon.AttacksPerSecond++  )
        },
        [6] = new ()
        {
            id = 6,
            title = "Збільшити радіус 'Пушки масового ураження'",
            description = () => "+20%",
            onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.canon.AttackRange *= 1.2f  )
        },
        [7] = new ()
        {
            id = 7,
            title = "Збільшити радіус взриву 'Пушки масового ураження'",
            description = () => "+40%",
            onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.canon.ExplosionRange *= 1.4f  )
        },
    };
    

    public class UpgradeData
    {
        public int id;
        public string title;
        public Func<string> description;
        public Sprite sprite;
        public Action onClickAction;
    }
}
