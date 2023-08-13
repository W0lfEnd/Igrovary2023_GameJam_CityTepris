using Clicker;
using System;
using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class BaseEnemy : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [Header("Don't touch this, this is SOLID")]
        public BoxCollider2D boxCollider;
        public bool isDragged = false;

        private int _health;
        private int _maxHealth;

        private int _defaultDamage;
        private int _currentDamage;
        private bool _isDamageIncreased;

        private float _defaultMovementSpeed;
        private float _currentMovementSpeed;
        private bool _isMovementSpeedIncreased;

        private Dimension _currentDimension;

        private GameObject _closestBuildingTarget;
        private BuildingsDistancer _buildingsDistancer;

        private void Awake() => Validate();

        private void Update() => TryToMoveTowardsClosestBuilding();

        private void OnCollisionEnter2D(Collision2D collision)
        {
            //do damage to the building, deactivate this object and then return to pool?
        }

        public void Initialize(EnemyData enemyData, Dimension spawnDimension, BuildingsDistancer buildingsDistancer)
        {
            SetData(enemyData, spawnDimension);
            ConfigurateTarget(buildingsDistancer);

            gameObject.SetActive(true);
            boxCollider.enabled = true;
        }

        public void SetActiveDragging(bool isActive)
        {
            isDragged = isActive;
            boxCollider.enabled = !isActive;
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

        private void Validate()
        {
            isDragged = false;
            boxCollider.enabled = false;
        }

        private void SetData(EnemyData enemyData, Dimension spawnDimension)
        {
            gameObject.name = enemyData.name;

            _spriteRenderer.sprite = enemyData.sprite;

            _maxHealth = enemyData.health;
            _health = enemyData.health;

            _defaultDamage = enemyData.damage;
            _currentDamage = enemyData.damage;

            _defaultMovementSpeed = enemyData.movementSpeed;
            _currentMovementSpeed = enemyData.movementSpeed;

            _currentDimension = spawnDimension;
        }

        private void ConfigurateTarget(BuildingsDistancer buildingsDistancer)
        {
            _buildingsDistancer = buildingsDistancer;
            _closestBuildingTarget = _buildingsDistancer.TryGetClosestBuilding(this, _currentDimension);
            _buildingsDistancer.onBuild += TrySetNewClosestBuilding;
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
            _buildingsDistancer.onBuild -= TrySetNewClosestBuilding;
            gameObject.SetActive(false);

            GameManager.Instance.xp += (int)(_maxHealth / 3f);
        }

        private IEnumerator DelayedAction(float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);

            callback.Invoke();
        }
    }

    public enum Dimension { TopDimesion, BottomDimension }
}
