using System;
using Assets.Scripts.Tools;
using UnityEngine;


    public class NewTimer : IDisposable
    {
        private readonly float _finishTime;
        private float _curentTime;
        private  Action _callback;
        private readonly bool _repeat;


        public NewTimer(float timer, Action method, bool repeat)
        {
            _finishTime = timer;
            _callback += method;
            _repeat = repeat;
            _curentTime = 0.0f;
        }

        public static void Add(float finishTime, Action method, bool repeat = false)
        {
            var timer = new NewTimer(finishTime, method, repeat);
            ProcessingTimer.Instantiate.Add(timer);
        }

        public void Tick()
        {
            _curentTime += Time.deltaTime;

            if (_curentTime > _finishTime)
            {
                _callback.Invoke();
               // Debug.Log("Callback name " + _callback.Target);
                
                if (_repeat)
                {
                    _curentTime = 0.0f;
                }

                else
                {
                    Kill();
                }
            }   
        }

        public void Stop()
        {
            ProcessingTimer.Instantiate.Remove(this);            
        }

        public void Play()
        {
            ProcessingTimer.Instantiate.Add(this);
        }


        void Kill()
        {
            ProcessingTimer.Instantiate.Remove(this);
            _callback = null;
        }

        public void Dispose()
        {
        }
    }
