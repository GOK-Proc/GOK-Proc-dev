using System.Collections.Generic;

namespace Rhythm
{
    public interface IActiveLaneProvider
    {
        IEnumerable<int> ActiveLanes { get; }
    }
}