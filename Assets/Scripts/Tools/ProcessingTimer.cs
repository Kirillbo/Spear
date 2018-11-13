using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tools
{
    public class ProcessingTimer
    {

        private static ProcessingTimer _instantiate;
        private List<NewTimer> _ticks;

        public static ProcessingTimer Instantiate
        {
            get
            {
                if (_instantiate == null)
                {
                    _instantiate = new ProcessingTimer();
                }
                return _instantiate;
            }
        }


        public ProcessingTimer()
        {
            _ticks = new List<NewTimer>();
        }


        public void Tick()
        {
            for (int i = 0; i < _ticks.Count; i++)
            {
                _ticks[i].Tick();
            }
        }

        public void Add(NewTimer timer)
        {
            _ticks.Add(timer);
        }

        public void Remove(NewTimer timer)
        {
            _ticks.Remove(timer);
        }
    }
}
