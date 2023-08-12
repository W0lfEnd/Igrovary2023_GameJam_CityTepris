using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class MapFigureGenerator : MonoBehaviour
{
    public event Action<MapFigureData> onNewFigure = delegate {};

    
    [SerializeField] private float generateEveryMs = 3000.0f;

    private bool  isGenerating = false;
    private float curTimerMs   = 0;

    public void startGenerating()
    {
        enabled = true;
        curTimerMs = 0.0f;
    }

    public void stopGenerating()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        curTimerMs += Time.deltaTime;

        if ( curTimerMs > generateEveryMs )
        {
            onNewFigure?.Invoke(new MapFigureData( getRandomFigureType() ));
            curTimerMs = 0.0f;
        }
    }

    private MapFigureData.Type getRandomFigureType()
    {
        return (MapFigureData.Type)Random.Range( (int)MapFigureData.Type.FIRST, (int)MapFigureData.Type.AFTER_LAST );
    }
}
