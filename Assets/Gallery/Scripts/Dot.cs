using UnityEngine.UI;
using UnityEngine.Pool;

namespace Gallery
{
    public class Dot : Image
    {
        private IObjectPool<Dot> _objectPool;

        public IObjectPool<Dot> ObjectPool
        {
            set => _objectPool = value;
        }
    };
}