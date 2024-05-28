using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public interface IObjectPoolProvider<T>
    {
        T Create();
        void Destroy(T obj);
        void Clear();
    }
}