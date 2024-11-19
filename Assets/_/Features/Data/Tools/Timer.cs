using System;
using UnityEngine;

namespace Data.Runtime
{
    public class Timer
    {
        public event Action OnTimerFinished;

        #region Methods

        public Timer(float duration)
        {
            _duration = duration;
        }

        public void Tick()
        {
            if (!_isRunning) return;

            _remainingTime -= Time.deltaTime;

            if (_remainingTime < 0)
            {
                Stop();
                OnTimerFinished?.Invoke();
            }
        }

        public void Reset()
        {
            _isRunning = false;
            _remainingTime = _duration;
        }

        public void Begin() => _isRunning = true;

        public void Stop() => _isRunning = false;

        public bool IsRunning() => _isRunning;

        public void UpdateTimer(float duration) => _duration = duration;

        public float GetRemainingTime() => _remainingTime;

        #endregion

        #region Privates

        private float _duration;

        private bool _isRunning;
        private float _remainingTime;

        #endregion
    }

}
