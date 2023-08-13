using Enemies;
using UnityEngine;

namespace Game.Scripts.GameBoardLogic.Board
{
    public class BoardTile : MonoBehaviour
    {
        [SerializeField] private GameObject particlePrefab;
        [SerializeField] private Sprite OverworldSprite;
        [SerializeField] private Sprite UnderworldSprite;

        public void OnBuilt(Dimension world)
        {
            if (particlePrefab != null)
                Instantiate(particlePrefab).transform.position = transform.position;

            var renderer = GetComponent<SpriteRenderer>();

            if (world == Dimension.TopDimesion)
                renderer.sprite = OverworldSprite;
            else
                renderer.sprite = UnderworldSprite;
        }
    }
}