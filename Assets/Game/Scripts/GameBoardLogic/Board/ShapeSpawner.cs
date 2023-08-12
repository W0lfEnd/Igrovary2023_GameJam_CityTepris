using System;
using System.Collections.Generic;
using Unity.Mathematics;
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

        public Action<ShapeSpawner> OnFigureSpawned;


        public void Start()
        {
            _random = new Unity.Mathematics.Random(1);
            SpawnedInstances = new List<GameObject>();
        }

        private void Update()
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            transform.position = worldPos;
            SpawnCoolDown -= Time.deltaTime;

            if (SpawnCoolDown <= 0.0f)
            {
            }
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

                GameObject instance = Instantiate(ItemPrefab, position, quaternion.identity, transform);

                SpawnedInstances.Add(instance);
            }
        }

        private BuildShape GenerateNextShape()
        {
            return GenerateShape(_random);
        }

        public BuildShape PeekNextShape()
        {
            return GenerateShape(new Unity.Mathematics.Random(_random.state));
        }

        private BuildShape GenerateShape(Unity.Mathematics.Random random)
        {
            int index = random.NextInt(Shapes.Length - 1);

            return Shapes[index];
        }
    }
}