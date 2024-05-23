using System.Collections.Generic;

namespace Rhythm
{
    public interface INoteProvider
    {
        bool ExistNotes { get; }
        IEnumerable<Note> Notes { get; }
    }
}