using System;
using Enemies;
using UnityEngine;

namespace Game.Scripts.GameBoardLogic.Board
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private HighlightTile highlightTilePrefab;
        [SerializeField] private Vector2Int BoardDimensions;
        [SerializeField] private int TileSize;

        [SerializeField] private Transform OverworldTilesRoot;
        [SerializeField] private Transform UnderworldTilesRoot;

        private BoardTile[,] _firstTiles;
        private BoardTile[,] _secondTiles;
        private HighlightTile[,] _highlightTiles;


        private void Start()
        {
            InitializeBoard();
        }

        private void OnDestroy()
        {
            foreach (var highlight in _highlightTiles)
            {
                Destroy(highlight);
            }
        }

        [ContextMenu(nameof(InitializeBoard))]
        public void InitializeBoard()
        {
            _firstTiles = new BoardTile[BoardDimensions.x, BoardDimensions.y];
            _secondTiles = new BoardTile[BoardDimensions.x, BoardDimensions.y];
            _highlightTiles = new HighlightTile[BoardDimensions.x, BoardDimensions.y];

            IterateABoard(_firstTiles, ((index, tile) =>
            {
                Vector3 position = GetTileWorldPositionByIndex(Dimension.TopDimesion, index);
                HighlightTile highlighter = Instantiate(highlightTilePrefab, position, Quaternion.identity, transform);

                _highlightTiles[index.x, index.y] = highlighter;

                highlighter.Hide();

                return true;
            }));
        }

        public void ClearHighlights()
        {
            foreach (var highlight in _highlightTiles)
            {
                highlight.Hide();
            }
        }

        public void HighlightAsAllowed(Vector2Int index)
        {
            _highlightTiles[index.x, index.y].HighlightAsAllowed();
        }

        public void HighlightAsDisAllowed(Vector2Int index)
        {
            _highlightTiles[index.x, index.y].HighlightAsDisAllowed();
        }

        public bool CanPlaceInTile(Dimension dimension, Vector3 worldPosition)
        {
            if (dimension == Dimension.BottomDimension)
                return false;

            Vector2Int? OptionalIndex = GetBoardIndexByWorldPosition(worldPosition);

            if (!OptionalIndex.HasValue)
                return false;

            Vector2Int Index = OptionalIndex.Value;

            bool isBlockedInOverworld = _firstTiles[Index.x, Index.y] != null;
            bool isBlockedInUnderworld = _secondTiles[Index.x, Index.y] != null;

            if (isBlockedInOverworld)
                return !isBlockedInUnderworld;
            else
                return true;
        }

        public bool SetNewTileIn(Dimension dimension, BoardTile tile, Vector2Int boardIndex)
        {
            if (dimension != Dimension.TopDimesion)
                return false;

            bool isIndexInsideABoard = boardIndex.x >= 0
                                       && boardIndex.y >= 0
                                       && boardIndex.x <= BoardDimensions.x
                                       && boardIndex.y <= BoardDimensions.y;

            if (!isIndexInsideABoard)
                return false;

            BoardTile[,] tiles;
            Transform parent;
            bool isBlockedInOverworld = _firstTiles[boardIndex.x, boardIndex.y] != null;
            bool isBlockedInUnderworld = _secondTiles[boardIndex.x, boardIndex.y] != null;

            if (isBlockedInOverworld)
            {
                if (isBlockedInUnderworld)
                    return false;

                tiles = _secondTiles;
                parent = UnderworldTilesRoot;
            }
            else
            {
                tiles = _firstTiles;
                parent = OverworldTilesRoot;
            }

            tiles[boardIndex.x, boardIndex.y] = tile;
            tile.transform.SetParent(parent);
            tile.transform.localPosition = new Vector3(boardIndex.x + 1, boardIndex.y + 1, 0);

            return true;
        }

        public Vector2Int? GetBoardIndexByWorldPosition(Vector3 WorldPosition)
        {
            Vector2Int? result = null;

            IterateABoard(_firstTiles, (index, tile) =>
            {
                Vector2 position = (Vector2)GetTileWorldPositionByIndex(Dimension.TopDimesion, index);

                if ((position - (Vector2)WorldPosition).magnitude <= TileSize / 2.0f)
                {
                    result = index;
                    return false;
                }

                return true;
            });

            return result;
        }

        public Vector3 GetTileWorldPositionByIndex(Dimension dimension, Vector2Int indexOnBoard)
        {
            indexOnBoard.x += 1;
            indexOnBoard.y += 1;

            Vector3 center;

            if (dimension == Dimension.TopDimesion)
                center = OverworldTilesRoot.position;
            else
                center = UnderworldTilesRoot.position;

            center += new Vector3(indexOnBoard.x * (TileSize), indexOnBoard.y * (TileSize), 0);

            return center;
        }

        private void IterateABoard(BoardTile[,] board, Func<Vector2Int, BoardTile, bool> action)
        {
            for (int y = 0; y < BoardDimensions.y; y++)
            {
                for (int x = 0; x < BoardDimensions.x; x++)
                {
                    BoardTile tile = board[x, y];

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
                Vector3 center = GetTileWorldPositionByIndex(Dimension.TopDimesion, index);
                Gizmos.DrawWireCube(center, new Vector3(TileSize, TileSize, TileSize));

                center = GetTileWorldPositionByIndex(Dimension.BottomDimension, index);
                Gizmos.DrawWireCube(center, new Vector3(TileSize, TileSize, TileSize));

                return true;
            });
        }
    }
}