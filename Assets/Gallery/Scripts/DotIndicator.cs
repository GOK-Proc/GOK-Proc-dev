using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Gallery
{
    public class DotIndicator : MonoBehaviour
    {
        private ObjectPool<Dot> _pool;

        private readonly List<Dot> _dots = new();

        [SerializeField] private Sprite _sprite;
        [SerializeField] private float _size = 16f;
        [SerializeField] private float _interval = 36f;
        [SerializeField] private Color _activeColor = Color.black;
        [SerializeField] private Color _inactiveColor = Color.gray;

        private void Awake()
        {
            _pool = new ObjectPool<Dot>(
                CreateDot,
                (Dot dot) => { dot.gameObject.SetActive(true); },
                (Dot dot) => { dot.gameObject.SetActive(false); },
                (Dot dot) => { Destroy(dot.gameObject); },
                true, 5, 10
            );
        }

        public void Init(int totalCount)
        {
            // _pool.Clear()が何も起こさない？ためforeachでベタ書き．
            foreach (var dot in _dots) _pool.Release(dot);
            _dots.Clear();

            if (totalCount < 2) return;

            var positionOffset = 0.0f - _interval * (totalCount - 1) / 2.0f;
            for (var i = 0; i < totalCount; i++)
            {
                var dot = _pool.Get();
                dot.sprite = _sprite;
                dot.color = i switch
                {
                    0 => _activeColor,
                    _ => _inactiveColor
                };
                _dots.Add(dot);

                var pos = new Vector2(positionOffset + _interval * i, 0f);
                dot.rectTransform.anchoredPosition = pos;
            }
        }

        /// <summary>
        /// 指定したインジケーターを点灯する．
        /// </summary>
        /// <param name="pageIndex">1-indexedであることに注意</param>
        public void Indicate(int pageIndex)
        {
            foreach (var dot in _dots) dot.color = _inactiveColor;
            _dots[pageIndex - 1].color = _activeColor;
        }

        private Dot CreateDot()
        {
            var dot = new GameObject("Dot").AddComponent<Dot>();
            dot.sprite = _sprite;
            dot.ObjectPool = _pool;

            dot.rectTransform.SetParent(gameObject.transform);
            dot.rectTransform.sizeDelta = new Vector2(_size, _size);

            return dot;
        }
    }
}