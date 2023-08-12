using UnityEngine;

namespace Clicker
{
    public class DamageClicker : MonoBehaviour
    {
        public int _currentClickDamage;

        private const int _defaultClickDamage = 1;

        private void Validate() //subscribe on action or event, when we start or restart the game
        {
            _currentClickDamage = _defaultClickDamage;
        }

        public void ApplyDamage(int additionalDamage)
        {
            _currentClickDamage += additionalDamage;
        }
    }
}
