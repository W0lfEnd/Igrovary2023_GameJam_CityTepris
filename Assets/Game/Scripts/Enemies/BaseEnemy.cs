using Clicker;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Enemies
{
    public class BaseEnemy : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private BoxCollider2D _boxCollider;

        private int _health;

        private int _defaultDamage;
        private int _currentDamage;
        private bool _isDamageIncreased;

        private float _defaultMovementSpeed;
        private float _currentMovementSpeed;
        private bool _isMovementSpeedIncreased;

        private Dimension _currentDimension;
        private GameObject _closestBuilding;
        private DamageClicker _damageClicker;

        private void Update()
        {
            TryToMoveTowardsClosestBuilding();
        }

        //to damage by click
        public void OnPointerEnter(PointerEventData eventData)
        {
            Damage(_damageClicker._currentClickDamage);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            //do damage to the building, destroy this object and then return to pool?
        }

        public void SetData(EnemyData enemyData, Dimension spawnDimension, DamageClicker damageClicker)
        {
            gameObject.name = enemyData.name;

            _spriteRenderer.sprite = enemyData.sprite;
            
            _health = enemyData.health;

            _defaultDamage = enemyData.damage;
            _currentDamage = enemyData.damage;

            _defaultMovementSpeed = enemyData.movementSpeed;
            _currentMovementSpeed = enemyData.movementSpeed;

            _currentDimension = spawnDimension;
        }

        public void Destroy()
        {
            gameObject.SetActive(false);
        }

        public void IncreaseHealth(int additionalHealth)
        {
            _health += additionalHealth;
        }

        public void IncreaseDamage(int additionalDamage, float boostLifeTime = 0f)
        {
            if (_isDamageIncreased)
                return;

            _currentDamage += additionalDamage;

            if (boostLifeTime != 0f)
            {
                _isDamageIncreased = true;

                DelayedAction(boostLifeTime, () =>
                {
                    _currentDamage = _defaultDamage;
                    _isDamageIncreased = false;
                });
            }
        }

        public void IncreaseSpeed(int additionalMovementSpeed, float boostLifeTime = 0f)
        {
            if (_isMovementSpeedIncreased)
                return;

            _currentMovementSpeed += additionalMovementSpeed;

            if (boostLifeTime != 0f)
            {
                _isDamageIncreased = true;

                DelayedAction(boostLifeTime, () =>
                {
                    _currentMovementSpeed = _defaultMovementSpeed;
                    _isMovementSpeedIncreased = false;
                });
            }
        }

        private void TryToMoveTowardsClosestBuilding()
        {
            if (_closestBuilding == null)
                return;

            Vector2 direction = _closestBuilding.transform.position - transform.position;
            direction.Normalize();

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.position = Vector2.MoveTowards(transform.position, _closestBuilding.transform.position, _currentMovementSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }

        public void Damage(int damage)
        {
            _health -= damage;

            if (_health <= 0)
                Destroy();
        }

        private IEnumerator DelayedAction(float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);

            callback.Invoke();
        }
    }

    public enum Dimension { TopDimesion, BottomDimension }
}
