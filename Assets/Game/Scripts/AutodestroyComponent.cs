using System;
using UnityEngine;

namespace Game.Scripts
{
    public class AutodestroyComponent : MonoBehaviour
    {
        [SerializeField] private float duration;
        private float startedAtTimestamp;

        private void OnEnable()
        {
            startedAtTimestamp = Time.time;
        }

        private void Update()
        {
            if (Time.time - startedAtTimestamp >= duration)
                Destroy(gameObject);
        }
    }
}