using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;


public class AllyBladeStorm : MonoBehaviour
{
    [SerializeField] private Transform            bladesRoot         = null;
    [SerializeField] private AllyBladeStorm_Blade go_blade           = null;
    [SerializeField] private int                  Damage             = 1;
    [SerializeField] private float                RotationSpeedInDeg = 10f;
    [SerializeField] private float                AttackRadius       = 10f;

    public int bladesCount
    {
        get => bladesList.Count;
        set => init( value );
    }

    private List<AllyBladeStorm_Blade> bladesList = new List<AllyBladeStorm_Blade>();

    private void OnEnable()
    {
        initBlades();
    }

    public void DisplayBlades(bool isActive)
    {
        gameObject.SetActive(isActive);

        if (isActive)
        {
            SoundsManager.Instance.TryPlaySoundByType(SoundType.BladeSpinning, true);
        }
    }

    public void init( int bladesCount = 5 )
    {
        for ( int i = 0; i < bladesList.Count; i++ )
            bladesList[i].gameObject.SetActive( true );
                    
        if ( bladesList.Count < bladesCount )
        {
            int bladesToSpawn = bladesCount - bladesList.Count;
            for ( int i = 0; i < bladesToSpawn; i++ )
            {
                AllyBladeStorm_Blade newBlade = Instantiate( go_blade, Vector3.zero, Quaternion.identity, bladesRoot ).GetComponent<AllyBladeStorm_Blade>();
                bladesList.Add( newBlade );
            }
        } else if ( bladesCount < bladesList.Count )
        {
            int bladesToDisable =  bladesList.Count - bladesCount;
            for ( int i = 0; i < bladesToDisable; i++ )
            {
                bladesList[i].gameObject.SetActive( false );
            }
        }

        initBlades();
    }
    
    private void initBlades()
    {
        if ( bladesList.Count == 0 )
            return;

        List<AllyBladeStorm_Blade> activeBlades      = bladesList.Where( it => it.isActiveAndEnabled ).ToList();
        float                      radianBladeOffset = 2 * Mathf.PI / activeBlades.Count;
        for ( int i = 0; i < activeBlades.Count; i++ )
        {
            activeBlades[i].init( Damage );
            activeBlades[i].transform.localPosition = AttackRadius * new Vector3( Mathf.Sin( i * radianBladeOffset ), Mathf.Cos( i * radianBladeOffset ), 0 );
            activeBlades[i].transform.right = (transform.position - activeBlades[i].transform.position).normalized;
        }
    }

    private void Update()
    {
        bladesRoot.Rotate( 0,0, Time.deltaTime * RotationSpeedInDeg );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere( transform.position, AttackRadius );
    }
}
