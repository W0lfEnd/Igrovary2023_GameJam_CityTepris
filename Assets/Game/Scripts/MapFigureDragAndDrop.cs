using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


public class MapFigureDragAndDrop : MonoBehaviour
{
    [SerializeField] private MapFigureGenerator figureGenerator = null;
    [SerializeField] private Tilemap tilemap = null;
    [SerializeField] private Tile figurePieceTile = null;

    private MapFigureData figureData = null;

    private void Awake()
    {
        figureGenerator.onNewFigure += setSelectedFigure;
        figureGenerator.onEnabled += isEnabled =>
        {
            if ( !isEnabled )
                tilemap.ClearAllTiles();
        };
        
        figureGenerator.startGenerating();
    }

    private void Update()
    {
        if ( figureData == null )
            return;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        tilemap.transform.position = new Vector3( worldPos.x, worldPos.y, tilemap.transform.position.z );
    }


    private void setSelectedFigure( MapFigureData figureData )
    {
        if ( this.figureData == figureData )
            return;

        if ( figureData == null )
        {
            tilemap.ClearAllTiles();
            return;
        }

        this.figureData = figureData;
        
        // 1, 0, 0
        // 1, 0, 1
        // 1, 1, 1

        tilemap.ClearAllTiles();
        for ( int i = figureData.shape.Count - 1; i >= 0; i-- )
        {
            for ( int j = 0; j < figureData.shape[i].Count; j++ )
            {
                if ( figureData.shape[i][j] == 0 )
                    continue;
        
                tilemap.SetTile( new Vector3Int( i, j, 0 ), figurePieceTile );
            }
        }

        tilemap.RefreshAllTiles();
    }
}
