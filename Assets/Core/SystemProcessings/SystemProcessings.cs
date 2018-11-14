using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


 public class SystemProcessings
    {
        private Dictionary<int, object> _data;
        private List<ITick> _listTicks;
        public bool SystemChange;

        public SystemProcessings()
        {
            Debug.Log("Init proc");
            _listTicks = new List<ITick>();
            _data = new Dictionary<int, object>();
            SystemChange = false;
        }

        public bool Contains<T>()
        {
            return _data.ContainsKey(typeof(T).GetHashCode());
        }


        public T Add<T>(Type type = null) where T : new()
        {
            object o;
            var hash = type == null ? typeof(T).GetHashCode() : type.GetHashCode();
            if (_data.TryGetValue(hash, out o))
            {
                InitializeObject(o);
                return (T)o;
            }

            var created = new T();

            InitializeObject(created);
            _data.Add(hash, created);

            return created;
        }


        public  object Get(Type t)
        {
            object resolve;
            _data.TryGetValue(t.GetHashCode(), out resolve);
            return resolve;
        }

//        public static object Add(object obj)
//        {
//            object possibleObj;
//            if (Instance.data.TryGetValue(obj.GetType().GetHashCode(), out possibleObj))
//            {
//                InitializeObject(possibleObj);
//                return possibleObj;
//            }
//
//            var add = obj;
//            var scriptable = obj as ScriptableObject;
//            if (scriptable) add = Instantiate(scriptable);
//            InitializeObject(obj);
//
//
//            Instance.data.Add(obj.GetType().GetHashCode(), add);
//            return add;
//        }

        public void Update()
        {
            if(SystemChange || _listTicks.Count == 0) return;

            for (int i = 0; i < _listTicks.Count; i++)
            {
                _listTicks[i].Tick();
            }
        }

        public void Remove(object obj)
        {
            _data.Remove(obj.GetType().GetHashCode());
        }

        public void InitializeObject(object obj)
        {
            var awakeble = obj as IAwake;
            if (awakeble != null) awakeble.OnAwake();

            var tickable = obj as ITick;
            if (tickable != null) _listTicks.Add(tickable);
        }


        public  T Get<T>()
        {
            object resolve;

            var hasValue = _data.TryGetValue(typeof(T).GetHashCode(), out resolve);

            if (!hasValue)
                _data.TryGetValue(typeof(T).GetHashCode(), out resolve);
            return (T)resolve;
        }

        public void Clear()
        {
            SystemChange = true;
            foreach (var pair in _data)
            { 
                var needToBeCleaned = pair.Value as IDisposable;
                if (needToBeCleaned == null) continue;
                needToBeCleaned.Dispose();
            }

            _listTicks.Clear();
            _data.Clear();

        }
    }
