using System.Collections.Generic;

namespace Rhythm
{
    public interface ILaneObjectProvider
    {
        bool ExistLaneObjects { get; }
        IEnumerable<LaneObject> LaneObjects { get; }
    }
}