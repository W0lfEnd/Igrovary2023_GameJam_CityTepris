using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.GameBoardLogic.Board
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private Image _tileIcon;

        public int X { get; private set; }
        public int Y { get; private set; }
        public TileType Type { get; private set; }

        public Image Icon => _tileIcon;

        public void Initialize(int x, int y)
        {
            Type = TileType.Opened;

            X = x;
            Y = y;
            transform.localScale = Vector3.one;
            _tileIcon.transform.localScale = Vector3.one;

            DOTween.Kill(transform);
            DOTween.Kill(_tileIcon);
        }

        public void ReInit()
        {
            Initialize(X,Y);
        }

        public void SetTileType(TileType tileType)
        {
            Type = tileType;
        }
    }
}