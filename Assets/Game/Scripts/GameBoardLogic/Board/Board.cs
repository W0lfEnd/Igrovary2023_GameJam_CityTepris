using System;
using System.Linq;
using UnityEngine;

namespace Game.Scripts.GameBoardLogic.Board
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private Row[] _rows;

        public int Height => _tiles.GetLength(1);
        public int Width => _tiles.GetLength(0);

        public Tile[,] Tiles => _tiles;

        private Tile[,] _tiles;

        private void Start()
        {
            //TOdo for test 
            InitializeBoard();
            
            _tiles[3,2].Icon.color=Color.red;
        }

        public void InitializeBoard()
        {
            _tiles = new Tile[_rows.Max(row => row.Tiles.Length), _rows.Length];
            
            InitializeTiles();
        }

        private void InitializeTiles()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var tile = _rows[y].Tiles[x];
                    
                    tile.Initialize(x,y);
                    
                    _tiles[x, y] = tile;
                }
            }
        }
    }
}