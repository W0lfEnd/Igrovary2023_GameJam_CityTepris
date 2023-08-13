using Enemies;
using UnityEngine;

namespace Game.Scripts.GameBoardLogic.Board
{
    public enum BuildType
    {
        BuiltOnFreeTile,
        BuiltOnAnotherTile
    }

    public class BoardTile : MonoBehaviour
    {
        [SerializeField] private GameObject buildParticlePrefab;
        [SerializeField] private GameObject linkParticlePrefab;
        [SerializeField] private Sprite OverworldSprite;
        [SerializeField] private Sprite UnderworldSprite;

        public void OnBuilt(Dimension world, BuildType buildType)
        {
            var renderer = GetComponent<SpriteRenderer>();
            GetComponent<Collider2D>().enabled = true;

            if (world == Dimension.TopDimesion)
                renderer.sprite = OverworldSprite;
            else
                renderer.sprite = UnderworldSprite;

            if (buildType == BuildType.BuiltOnFreeTile)
                OnBuildInFreeTile();
            if (buildType == BuildType.BuiltOnAnotherTile)
                OnBuildOnAnotherTile();
        }

        private void OnBuildInFreeTile()
        {
            if (buildParticlePrefab != null)
                Instantiate(buildParticlePrefab).transform.position = transform.position;
        }

        private void OnBuildOnAnotherTile()
        {
            if (buildParticlePrefab != null)
                Instantiate(buildParticlePrefab).transform.position = transform.position;
        }
    }
}