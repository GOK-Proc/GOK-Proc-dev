using System;
using System.Collections.Generic;
using UnityEngine;
using FancyScrollView;
using EasingCore;
using Gallery;

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
            Context.OnCellSelected += ScrollTo;
            _onSelectionChanged?.Invoke(Context.SelectedIndex);
        }

        public void OnSelectionChanged(Action<int> callback)
        {
            _onSelectionChanged = callback;
        }

        public void UpdateData(IList<TrackInformation> items)
        {
            UpdateContents(items);
            Scroller.SetTotalCount(items.Count);
        }

        public void JumpTo(int index)
        {
            UpdateSelection(index);
            JumpTo(index, 1.0f);
        }

        private void ScrollTo(int index)
        {
            UpdateSelection(index);
            // ScrollToのdurationは，InputSystemUiInputModuleのMoveRepeatRateに合わせた
            ScrollTo(index, 0.1f, Ease.InOutQuint, 1.0f);
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