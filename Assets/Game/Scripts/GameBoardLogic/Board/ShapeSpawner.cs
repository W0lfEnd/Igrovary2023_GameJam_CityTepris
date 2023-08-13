using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Game.Scripts.GameBoardLogic.Board
{
    [Serializable]
    public struct BuildShape
    {
        public string debugName;
        public Vector2Int[] points;
    }

    public class ShapeSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject ItemPrefab;
        [SerializeField] private BuildShape[] Shapes;
        [SerializeField] private float SpawnCoolDown;

        private Unity.Mathematics.Random _random;
        private List<GameObject> SpawnedInstances;
        private float _lastTimeSpawned;
        private Camera camera;
        private Board _board;

        public static Action<ShapeSpawner> OnFigureSpawned;


        public void Start()
        {
            _random = new Unity.Mathematics.Random(1);
            SpawnedInstances = new List<GameObject>();
            _lastTimeSpawned = Time.time;
            camera = Camera.main;
            _board = FindObjectOfType<Board>();
        }

        private void Update()
        {
            UpdatePositionToMouse();

            bool canPlaceWholeDistrict = HoverShapesOverABoard();

            if (canPlaceWholeDistrict)
                BuildDistrictShape();

            if (SpawnedInstances.Count > 0)
                _lastTimeSpawned = Time.time;

            if (Time.time - _lastTimeSpawned < SpawnCoolDown)
                return;

            _lastTimeSpawned = Time.time;

            SpawnNextShape();
        }

        private void UpdatePositionToMouse()
        {
            Vector3 worldPos = camera.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;
            transform.position = worldPos;
        }

        private bool HoverShapesOverABoard()
        {
            if (GameManager.Instance.IsUpgradesPanelActive())
                return false;

            _board.ClearHighlights();

            bool canPlaceWholeShape = true;

            if (SpawnedInstances.Count == 0)
            {
                _board.ClearHighlights();
                return false;
            }

            foreach (GameObject shapeInstance in SpawnedInstances)
            {
                Vector3 position = shapeInstance.transform.localPosition + transform.position;
                bool canPlaceTile = _board.CanPlaceInTile(GameManager.Instance.Dimension, position);
                Vector2Int? optionalIndex =
                    _board.GetBoardIndexByWorldPosition(position);

                if (!canPlaceTile)
                {
                    canPlaceWholeShape = false;

                    if (optionalIndex.HasValue)
                        _board.HighlightAsDisAllowed(optionalIndex.Value);
                }
                else
                {
                    if (optionalIndex.HasValue)
                        _board.HighlightAsAllowed(optionalIndex.Value);
                }
            }

            return canPlaceWholeShape;
        }

        private void BuildDistrictShape()
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            for (int i = SpawnedInstances.Count - 1; i >= 0; i--)
            {
                GameObject instance = SpawnedInstances[i];
                Vector3 position = instance.transform.localPosition + transform.position;
                Vector2Int? optionalIndex =
                    _board.GetBoardIndexByWorldPosition(position);

                if (optionalIndex.HasValue)
                {
                    _board.SetNewTileIn(GameManager.Instance.Dimension, instance.GetComponent<BoardTile>(),
                        optionalIndex.Value);
                    SpawnedInstances.RemoveAt(i);
                }
            }

            foreach (var instance in SpawnedInstances)
            {
                Destroy(instance);
            }

            SpawnedInstances.Clear();
            _board.NotifyBoardChanged();
        }

        private void SpawnNextShape()
        {
            foreach (var oldInstance in SpawnedInstances)
            {
                Destroy(oldInstance);
            }

            BuildShape shape = GenerateNextShape();

            foreach (Vector2Int point in shape.points)
            {
                Vector3 position = new Vector3(point.x, point.y, 0.0f);

                GameObject instance = Instantiate(ItemPrefab, transform);
                instance.transform.localPosition = position;

                SpawnedInstances.Add(instance);
            }

            OnFigureSpawned?.Invoke(this);
        }

        private BuildShape GenerateNextShape()
        {
            return GenerateShape(ref _random);
        }

        public BuildShape PeekNextShape()
        {
            Unity.Mathematics.Random randomCopy = _random;
            return GenerateShape(ref randomCopy);
        }

        private BuildShape GenerateShape(ref Unity.Mathematics.Random random)
        {
            int index = random.NextInt(Shapes.Length);

            return Shapes[index];
        }
    }
}