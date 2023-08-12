using UnityEngine;

namespace Game.Scripts.GameBoardLogic.Board
{
    public class Row : MonoBehaviour
    {
        [SerializeField] private Tile[] _tiles;

        public Tile[] Tiles => _tiles;
    }
}