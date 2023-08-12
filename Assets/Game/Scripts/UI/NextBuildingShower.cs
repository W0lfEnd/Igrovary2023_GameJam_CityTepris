using Game.Scripts.GameBoardLogic.Board;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class NextBuildingShower : MonoBehaviour
    {
        [SerializeField] private BuildingVectors[] _buildingVectors;
        [SerializeField] private Sprite _buildingSprite;
        private Color _emptyColor = new Color(0f, 0f, 0f, 0f);
        private Color _normalColor = new Color(255f, 255f, 255f, 1f);

        private void Awake()
        {
            ShapeSpawner.OnFigureSpawned += (spawner) => Show(spawner.PeekNextShape());
        }

        private void Show(BuildShape buildingShape)
        {
            Clear();
            ConfigurateTiles(buildingShape.points);
        }

        private void ConfigurateTiles(Vector2Int[] vectors)
        {
            for (int i = 0; i < _buildingVectors.Length; i++)
            {
                for (int j = 0; j < vectors.Length; j++)
                {
                    if (_buildingVectors[i].buildingVector == vectors[j])
                        _buildingVectors[i].imageComponent.color = _normalColor;
                }
            }
        }

        private void Clear()
        {
            for (int i = 0; i < _buildingVectors.Length; i++)
            {
                _buildingVectors[i].imageComponent.color = _emptyColor;
            }
        }
    }

    [Serializable]
    public struct BuildingVectors
    {
        public Vector2Int buildingVector;
        public Image imageComponent;
    }
}
