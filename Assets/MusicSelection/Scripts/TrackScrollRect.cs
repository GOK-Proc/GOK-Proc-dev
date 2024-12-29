using System;
using System.Collections.Generic;
using UnityEngine;
using FancyScrollView;
using EasingCore;
using Gallery;
using UnityEngine.Serialization;

namespace MusicSelection
{
    public class TrackScrollRect : FancyScrollRect<TrackInformation, TrackContext>
    {
        [SerializeField] private float _cellSize = 70f;
        [SerializeField] private Scroller _scroller;
        [SerializeField] private GameObject _cellPrefab;

        private Action<int> _onSelectionChanged;

        protected override float CellSize => _cellSize;
        protected override GameObject CellPrefab => _cellPrefab;

        private void Start()
        {
            _scroller.OnValueChanged(UpdatePosition);
            Context.OnCellSelected += ScrollTo;
        }

        public void UpdateData(IList<TrackInformation> items)
        {
            UpdateContents(items);
            Scroller.SetTotalCount(items.Count);
        }

        public void ScrollToNext()
        {
            if (Context.SelectedIndex == ItemsSource.Count - 1) return;
            ScrollTo(Context.SelectedIndex + 1);
        }

        public void ScrollToPrevious()
        {
            if (Context.SelectedIndex == 0) return;
            ScrollTo(Context.SelectedIndex - 1);
        }

        private void ScrollTo(int index)
        {
            UpdateSelection(index);
            ScrollTo(index, 0.5f, Ease.InOutQuint, .5f);
        }

        public void JumpTo(int index)
        {
            UpdateSelection(index);
            JumpTo(index, .5f);
        }

        public void OnSelectionChanged(Action<int> callback)
        {
            _onSelectionChanged = callback;
        }

        private void UpdateSelection(int index)
        {
            if (Context.SelectedIndex == index) return;

            Context.SelectedIndex = index;

            _onSelectionChanged?.Invoke(index);

            Refresh();
        }
    }
}