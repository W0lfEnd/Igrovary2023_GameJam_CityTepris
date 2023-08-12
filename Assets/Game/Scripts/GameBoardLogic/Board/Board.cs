﻿using System;
using Enemies;
using UnityEngine;

namespace Game.Scripts.GameBoardLogic.Board
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private Vector2Int BoardDimensions;
        [SerializeField] private int TileSize;

        private BoardTile[,] _firstTiles;
        private BoardTile[,] _secondTiles;


        private void Start()
        {
            InitializeBoard();
        }

        public void InitializeBoard()
        {
            _firstTiles = new BoardTile[BoardDimensions.x, BoardDimensions.y];
            _secondTiles = new BoardTile[BoardDimensions.x, BoardDimensions.y];
        }

        public bool CanPlaceInTile(Dimension dimension, Vector3 worldPosition)
        {
            Vector2Int? OptionalIndex = GetBoardIndexByWorldPosition(worldPosition);

            if (!OptionalIndex.HasValue)
                return false;

            Vector2Int Index = OptionalIndex.Value;

            bool isBlockedInOverworld = _firstTiles[Index.x, Index.y] != null;
            bool isBlockedInUnderworld = _secondTiles[Index.x, Index.y] != null;

            if (dimension == Dimension.TopDimesion)
                return isBlockedInUnderworld;
            else
                return isBlockedInOverworld;
        }

        public bool SetNewTileIn(Dimension dimension, BoardTile tile, Vector2Int boardIndex)
        {
            BoardTile[,] tiles;

            if (dimension == Dimension.TopDimesion)
                tiles = _firstTiles;
            else
                tiles = _secondTiles;

            bool isIndexInsideABoard = boardIndex.x > 0
                                       && boardIndex.y > 0
                                       && boardIndex.x <= BoardDimensions.x
                                       && boardIndex.y <= BoardDimensions.y;
            if (!isIndexInsideABoard)
                return false;

            tiles[boardIndex.x, boardIndex.y] = tile;

            return true;
        }

        public Vector2Int? GetBoardIndexByWorldPosition(Vector3 WorldPosition)
        {
            Vector2Int? result = null;

            IterateABoard(_firstTiles, (index, tile) =>
            {
                Vector2 position = (Vector2)GetTileWorldPositionByIndex(index);

                if ((position - (Vector2)WorldPosition).magnitude <= TileSize / 2.0f)
                {
                    result = index;
                    return false;
                }

                return true;
            });

            return result;
        }

        public Vector3 GetTileWorldPositionByIndex(Vector2Int indexOnBoard)
        {
            indexOnBoard.x += 1;
            indexOnBoard.y += 1;

            Vector3 center = transform.position;
            center += new Vector3(indexOnBoard.x * (TileSize / 2.0f), indexOnBoard.y * (TileSize / 2.0f), 0);

            return center;
        }

        private void IterateABoard(BoardTile[,] board, Func<Vector2Int, BoardTile, bool> action)
        {
            for (int y = 0; y < BoardDimensions.y; y++)
            {
                for (int x = 0; x < BoardDimensions.x; x++)
                {
                    BoardTile tile = board[y, x];

                    if (!action(new Vector2Int(x, y), tile))
                        return;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_secondTiles == null || _firstTiles == null)
                InitializeBoard();

            IterateABoard(_firstTiles, (Vector2Int index, BoardTile tile) =>
            {
                Vector3 center = GetTileWorldPositionByIndex(index);

                Gizmos.DrawWireCube(center, new Vector3(TileSize, TileSize, TileSize));

                return true;
            });
        }
    }
}