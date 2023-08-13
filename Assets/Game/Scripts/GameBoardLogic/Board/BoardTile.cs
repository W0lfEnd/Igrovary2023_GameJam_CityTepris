using Enemies;
using UnityEngine;

namespace Game.Scripts.GameBoardLogic.Board
{
    public class BoardTile : MonoBehaviour
    {
        [SerializeField] private GameObject buildParticlePrefab;
        [SerializeField] private GameObject linkParticlePrefab;
        [SerializeField] private Sprite OverworldSprite;
        [SerializeField] private Sprite UnderworldSprite;

        public void OnBuilt(Dimension world)
        {
            var renderer = GetComponent<SpriteRenderer>();
            GetComponent<Collider2D>().enabled = true;

            if (world == Dimension.TopDimesion)
                renderer.sprite = OverworldSprite;
            else
                renderer.sprite = UnderworldSprite;

            if (buildParticlePrefab != null)
                Instantiate(buildParticlePrefab).transform.position = transform.position;
        }

        public void OnLinked()
        {
            if (linkParticlePrefab != null)
                Instantiate(linkParticlePrefab).transform.position = transform.position;
        }
    }
}