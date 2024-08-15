using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Map
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MapNavPoint : Selectable
    {
        [SerializeField] private float _size = 1f;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        protected override void Awake()
        {
            base.Awake();

            _spriteRenderer = GetComponent<SpriteRenderer>();
            UnityAction<SpriteRenderer> action = AdjustSpriteSize;
            _spriteRenderer.RegisterSpriteChangeCallback(action);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            AdjustSpriteSize(_spriteRenderer);
        }
#endif

        private void AdjustSpriteSize(SpriteRenderer spriteRenderer)
        {
            Vector3 originalSize = spriteRenderer.sprite.bounds.size;
            Vector3 adjustedSize = new Vector3(_size / originalSize.x, _size / originalSize.y, 1f);
            transform.localScale = adjustedSize;
        }
    }
}