using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;


public class UIUpgradePanel : MonoBehaviour
{
    [SerializeField] private UIBtnSelectUpgrade btnFirst  = null;
    [SerializeField] private UIBtnSelectUpgrade btnSecond = null;
    [SerializeField] private List<Sprite>       sprites   = null;

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
        Dictionary<int, UpgradeData> mocks = new  Dictionary<int, UpgradeData>()
        {
            [0] = new ()
            {
                id = 0,
                sprite = sprites[1],
                title = "Ріжучі леза",
                description = () => "Леза, що будуть крутитись покругу центрального будинку",
                onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.blade_storm.gameObject.SetActive( true ) )
            },
            [1] = new ()
            {
                id = 1,
                sprite = sprites[4],
                title = "Нова пушка",
                description = () => "Потужна пушка, яка стріляє вибуховими снарядами",
                onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.canon.gameObject.SetActive( true ) )
            },
            [2] = new ()
            {
                id = 2,
                sprite = sprites[0],
                title = "Додаткове лезо (+1)",
                description = () => $"наявна кількість лез: {GameManager.Instance.TurretControllers[0].blade_storm.bladesCount}",
                onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.blade_storm.bladesCount++ )
            },
            [3] = new ()
            {
                id = 3,
                sprite = sprites[6],
                title = "Пришвидшити 'Арбалету'",
                description = () => "+1 в секунду",
                onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.turret.AttacksPerSecond++  )
            },
            [4] = new ()
            {
                id = 4,
                sprite = sprites[5],
                title = "Збільшити радіус 'Арбалету'",
                description = () => "+20%",
                onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.turret.AttackRange *= 1.2f  )
            },
            [5] = new ()
            {
                id = 5,
                sprite = sprites[7],
                title = "Пришвидшити 'Пушку'",
                description = () => "+1 в секунду",
                onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.canon.AttacksPerSecond++  )
            },
            [6] = new ()
            {
                id = 6,
                sprite = sprites[7],
                title = "Пришвидшити 'Пушку'",
                description = () => "+1 в секунду",
                onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.canon.AttacksPerSecond++  )
            },
            [7] = new ()
            {
                id = 7,
                sprite = sprites[7],
                title = "Пришвидшити 'Пушку'",
                description = () => "+1 в секунду",
                onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.canon.AttacksPerSecond++  )
            },
            [8] = new ()
            {
                id = 8,
                sprite = sprites[3],
                title = "Збільшити радіус 'Пушки'",
                description = () => "+20%",
                onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.canon.AttackRange *= 1.2f  )
            },
            [9] = new ()
            {
                id = 9,
                sprite = sprites[3],
                title = "Збільшити радіус 'Пушки'",
                description = () => "+20%",
                onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.canon.AttackRange *= 1.2f  )
            },
            [10] = new ()
            {
                id = 10,
                sprite = sprites[2],
                title = "Збільшити вибух 'Пушки'",
                description = () => "+40%",
                onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.canon.ExplosionRange *= 1.4f  )
            },
            [11] = new ()
            {
                id = 11,
                sprite = sprites[5],
                title = "Збільшити радіус 'Арбалету'",
                description = () => "+20%",
                onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.turret.AttackRange *= 1.2f  )
            },
            [12] = new ()
            {
                id = 12,
                sprite = sprites[5],
                title = "Збільшити радіус 'Арбалету'",
                description = () => "+20%",
                onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.turret.AttackRange *= 1.2f  )
            },
            [13] = new ()
            {
                id = 13,
                sprite = sprites[5],
                title = "Збільшити радіус 'Арбалету'",
                description = () => "+20%",
                onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.turret.AttackRange *= 1.2f  )
            },
            [14] = new ()
            {
                id = 14,
                sprite = sprites[6],
                title = "Пришвидшити 'Арбалету'",
                description = () => "+1 в секунду",
                onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.turret.AttacksPerSecond++  )
            },
            [15] = new ()
            {
                id = 15,
                sprite = sprites[6],
                title = "Пришвидшити 'Арбалету'",
                description = () => "+1 в секунду",
                onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.turret.AttacksPerSecond++  )
            },
            [16] = new ()
            {
                id = 16,
                sprite = sprites[1],
                title = "Збільшити радіус 'Ріжучих лез'",
                description = () => "+30%",
                onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.blade_storm.AttackRadius *= 1.3f )
            },
            [17] = new ()
            {
                id = 17,
                sprite = sprites[1],
                title = "Пришвидшити 'Ріжучі леза'",
                description = () => "+30%",
                onClickAction = () => GameManager.Instance.TurretControllers.ForEach( it => it.blade_storm.RotationSpeedInDeg *= 1.3f )
            },
            
        };

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

            case 8: return turretsController.canon.gameObject.activeSelf;
            case 9: return turretsController.canon.gameObject.activeSelf;
            case 10: return turretsController.canon.gameObject.activeSelf;

            case 16: return turretsController.blade_storm.gameObject.activeSelf;
            case 17: return turretsController.blade_storm.gameObject.activeSelf;
        }

        return true;
    }


    public class UpgradeData
    {
        public int id;
        public string title;
        public Func<string> description;
        public Sprite sprite;
        public Action onClickAction;
    }
}
