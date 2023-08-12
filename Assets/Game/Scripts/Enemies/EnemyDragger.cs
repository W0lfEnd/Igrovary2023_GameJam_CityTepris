using UnityEngine;
using UnityEngine.EventSystems;

namespace Enemies
{
    public class EnemyDragger : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private BaseEnemy _enemy;
        private Camera _mainCamera;
        private Vector2 _mousePosition;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _enemy.isDragged = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _mousePosition = _mainCamera.ScreenToWorldPoint(eventData.position);
            gameObject.transform.position = _mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _enemy.isDragged = false;
        }
    }
}
