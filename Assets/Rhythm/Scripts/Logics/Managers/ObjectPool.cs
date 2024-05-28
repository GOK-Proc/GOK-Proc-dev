using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Rhythm
{
    public class ObjectPool<T> : IObjectPoolProvider<T> where T : RhythmGameObject
    {
        private readonly Stack<T> _pool;
        private readonly T _obj;
        private readonly Transform _parent;
        private readonly Action<RhythmGameObject> _onInstantiate;

        public ObjectPool(T obj, Transform parent, Action<RhythmGameObject> onInstantiate = null, int initNum = 0)
        {
            _pool = new Stack<T>();
            _obj = obj;
            _parent = parent;
            _onInstantiate = onInstantiate;

            for (int i = 0; i < initNum; i++)
            {
                _pool.Push(UnityEngine.Object.Instantiate(_obj, _parent));
            }
        }

        public T Create()
        {
            if (_pool.Count > 0)
            {
                return _pool.Pop();
            }
            else
            {
                var obj = UnityEngine.Object.Instantiate(_obj, _parent);
                _onInstantiate?.Invoke(obj);
                return obj;
            }
        }

        public PooledObject<T> Create(out T obj, out bool isNew)
        {
            if (_pool.Count > 0)
            {
                obj = _pool.Pop();
                isNew = false;
            }
            else
            {
                obj = UnityEngine.Object.Instantiate(_obj, _parent);
                _onInstantiate?.Invoke(obj);
                isNew = true;
            }
            return new PooledObject<T>(obj, this);
        }

        public void Destroy(T obj)
        {
            _pool.Push(obj);
        }

        public void Clear()
        {
            while (_pool.Count > 0)
            {
                var obj = _pool.Pop();
                UnityEngine.Object.Destroy(obj);
            }
        }

    }
}