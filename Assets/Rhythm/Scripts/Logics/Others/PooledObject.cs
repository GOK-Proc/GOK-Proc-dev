using System;
using UnityEngine;

namespace Rhythm
{
    public class PooledObject<T> : IDisposable where T : MonoBehaviour
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
