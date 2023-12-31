using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class MapFigureGenerator : MonoBehaviour
{
    public event Action<MapFigureData> onNewFigure = delegate {};
    public event Action<bool> onEnabled = delegate {};

    
    [SerializeField] private float generateEveryMs = 3000.0f;

    private bool  isGenerating = false;
    private float curTimerMs   = 0;

    public void startGenerating()
    {
        enabled = true;
        curTimerMs = 0.0f;
        onEnabled( true );
    }

    public void stopGenerating()
    {
        enabled = false;
        onEnabled( false );
    }

    // Update is called once per frame
    void Update()
    {
        curTimerMs += Time.deltaTime * 1000;

        if ( curTimerMs > generateEveryMs )
        {
            curTimerMs = 0.0f;
            onNewFigure?.Invoke(new MapFigureData( getRandomFigureType() ));
        }
    }

    private MapFigureData.Type getRandomFigureType()
    {
        return (MapFigureData.Type)Random.Range( (int)MapFigureData.Type.FIRST, (int)MapFigureData.Type.AFTER_LAST );
    }
}
