using System;

namespace UT.GameLogic
{
    public class Life
    {
        private float _totalLife = 0;
        private float _currentLife = 0;

        private Action _OnDead;

        public float TotalLife => _totalLife;
        public float CurrentLife => _currentLife;

        public Life(float totalLife, Action OnDead)
        {
            _OnDead += OnDead;
            _totalLife = totalLife;
            _currentLife = totalLife;
        }

        public void GetDamage(float damage)
        {
            _currentLife -= damage;
            if (_currentLife <= 0) _OnDead?.Invoke();
        }
    }
}