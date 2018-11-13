using System;
using UnityEngine;


namespace Tools
{
    public class Timer : IDisposable
    {
        readonly Action CallBack;

        protected float _currentTime, Time, SecondTime;

        public Timer(float permamentTime, float timer, Action method)
        {
            Time = permamentTime;
            _currentTime = 0;
            SecondTime = timer;
            CallBack += method;

        }



        public Timer(float time, Action method)
        {
            Time = time;
            _currentTime = 0f;
            CallBack += method;
        }

        public virtual bool Tick(float deltaTime)
        {
            return AssessTime(deltaTime);
        }

        protected virtual bool AssessTime(float deltaTime)
        {
            _currentTime += deltaTime;

            if (_currentTime >= Time)
            {
                FiredEvent();
                return true;
            }

            return false;
        }

        void FiredEvent()
        {
            CallBack.Invoke();
        }

        public void Dispose()
        {

        }
    }
}