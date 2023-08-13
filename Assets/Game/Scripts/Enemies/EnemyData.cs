using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(menuName = "Create new enemy")]
    public class EnemyData : ScriptableObject
    {
        public string _name;
        public Sprite sprite;
        public AnimationClip animationClip;
        public int health;
        public int damage;
        public float movementSpeed;
    }
}
