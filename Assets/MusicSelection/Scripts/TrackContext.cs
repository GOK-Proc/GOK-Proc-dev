using System;
using FancyScrollView;

namespace MusicSelection
{
    public class TrackContext : FancyScrollRectContext
    {
        public int SelectedIndex = -1;
        public Action<int> OnCellSelected;
    }
}