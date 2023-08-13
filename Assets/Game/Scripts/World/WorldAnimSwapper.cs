using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts.World
{
    public class WorldAnimSwapper : MonoBehaviour
    {
        [SerializeField] private Camera _targetCamera;
        [SerializeField] private Transform _to;
        [SerializeField] private float _duration;

        public Action OnCompleteSwap;
        public Action OnStartSwap;

        public void SwapToDown()
        {
            OnStartSwap?.Invoke();
            DOTween.Kill(_targetCamera.transform);

            MoveYAnimation(_targetCamera.gameObject, _duration, 0, _to.position.y, () => { OnCompleteSwap?.Invoke(); });
            //RotateAnimation(_targetCamera.gameObject, _duration, new Vector3(0, 0, -180), (() => { }));
        }

        public void SwapToUp()
        {
            OnStartSwap?.Invoke();
            DOTween.Kill(_targetCamera.transform);

            MoveYAnimation(_targetCamera.gameObject, _duration, 0, 0, (() => { OnCompleteSwap?.Invoke(); }));
            //_targetCamera.transform.localRotation= Quaternion.Euler(0f, 0f, 180f);
            //RotateAnimation(_targetCamera.gameObject, _duration, new Vector3(0, 0, 0), (() => { }));
        }

        public void MoveYAnimation(GameObject gameObject, float duration, float delay, float yPosition,
            Action onComplete)
        {
            gameObject.transform.DOMoveY(yPosition, duration).SetEase(Ease.OutQuad)
                .SetDelay(delay)
                .OnComplete(() => { onComplete?.Invoke(); });
        }

        public void RotateAnimation(GameObject gameObject, float duration, Vector3 rotate, Action onComplete)
        {
            gameObject.transform.DORotate(rotate, duration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => { onComplete?.Invoke(); });
        }
    }
}