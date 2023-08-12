using Clicker;
using System;
using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class BaseEnemy : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private BoxCollider2D _boxCollider;
        [Header("Don't touch this, this is SOLID")]
        public bool isDragged = false;

        private int _health;

        private int _defaultDamage;
        private int _currentDamage;
        private bool _isDamageIncreased;

        private float _defaultMovementSpeed;
        private float _currentMovementSpeed;
        private bool _isMovementSpeedIncreased;

        private Dimension _currentDimension;

        private GameObject _closestBuildingTarget;
        private BuildingsDistancer _buildingsDistancer;

        private void Awake()
        {
            gameObject.SetActive(false);
            _boxCollider.enabled = false;
        }

        private void Update()
        {
            TryToMoveTowardsClosestBuilding();
        }

        public void SetClosestBuilding(GameObject target)
        {
            _closestBuildingTarget = target;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            //do damage to the building, deactivate this object and then return to pool?
        }

        public void Initialize(EnemyData enemyData, Dimension spawnDimension, DamageClicker damageClicker, BuildingsDistancer buildingsDistancer)
        {
            isDragged = false;
            _boxCollider.enabled = false;

            SetData(enemyData, spawnDimension);
            ConfigurateTarget(buildingsDistancer);

            gameObject.SetActive(true);
            _boxCollider.enabled = true;
        }

        private void SetData(EnemyData enemyData, Dimension spawnDimension)
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

        private void ConfigurateTarget(BuildingsDistancer buildingsDistancer)
        {
            _closestBuildingTarget = buildingsDistancer.TryGetClosestBuilding(this, _currentDimension);
            buildingsDistancer.onBuild += TrySetNewClosestBuilding;
        }

        public void Damage(int damage)
        {
            _health -= damage;

            if (_health <= 0)
            {
                Destroy();
            }
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
            if (isDragged)
                return;

            if (_closestBuildingTarget == null)
                return;

            Vector2 direction = _closestBuildingTarget.transform.position - transform.position;
            direction.Normalize();

            //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.position = Vector2.MoveTowards(transform.position, _closestBuildingTarget.transform.position, _currentMovementSpeed * Time.deltaTime);
            //transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }

        private void TrySetNewClosestBuilding(Dimension buildingDimension)
        {
            if (buildingDimension != _currentDimension)
                return;

            _closestBuildingTarget = _buildingsDistancer.TryGetClosestBuilding(this, _currentDimension);
        }

        private void Destroy()
        {
            _buildingsDistancer.onBuild += TrySetNewClosestBuilding;
            gameObject.SetActive(false);
        }

        private IEnumerator DelayedAction(float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);

            callback.Invoke();
        }
    }

    public enum Dimension { TopDimesion, BottomDimension }
}
