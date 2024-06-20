using System.Collections.Generic;

namespace Rhythm
{
    public interface IRhythmGameObjectProvider
    {
        IEnumerable<RhythmGameObject> RhythmGameObjects { get; }
    }
}