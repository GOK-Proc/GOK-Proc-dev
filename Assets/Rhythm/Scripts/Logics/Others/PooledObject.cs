using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Rhythm
{
    public class PooledObject<T> : IDisposable where T : RhythmGameObject
    {
        private readonly T _obj;
        private readonly IObjectPoolProvider<T> _pool;

        public PooledObject(T obj, IObjectPoolProvider<T> pool)
        {
            _obj = obj;
            _pool = pool;
        }

        public void Dispose() => _pool.Destroy(_obj);
    }
}