using System.Collections.Generic;

namespace Rhythm
{
    public interface INoteProvider
    {
        IEnumerable<Note> Notes { get; }
    }
}