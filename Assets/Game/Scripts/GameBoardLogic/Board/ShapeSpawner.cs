using System;
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
        [SerializeField] private BuildShape[] Shapes;
        [SerializeField] private float SpawnCoolDown;

        private Unity.Mathematics.Random _random;

        public void Start()
        {
            _random = new Unity.Mathematics.Random(1);
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