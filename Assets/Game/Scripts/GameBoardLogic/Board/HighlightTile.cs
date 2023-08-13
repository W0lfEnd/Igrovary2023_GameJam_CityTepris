using UnityEngine;

namespace Game.Scripts.GameBoardLogic.Board
{
    public class HighlightTile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer renderer;
        [SerializeField] private Sprite AllowedSprite;
        [SerializeField] private Sprite DisAllowedSprite;

        public void HighlightAsAllowed()
        {
            gameObject.SetActive(true);

            renderer.sprite = AllowedSprite;
        }

        public void HighlightAsDisAllowed()
        {
            gameObject.SetActive(true);

            GetComponent<SpriteRenderer>().sprite = DisAllowedSprite;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}