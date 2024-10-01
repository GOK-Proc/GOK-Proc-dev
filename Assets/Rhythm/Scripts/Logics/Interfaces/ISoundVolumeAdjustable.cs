namespace Rhythm
{
    public interface ISoundVolumeAdjustable
    {
        float BgmVolume { get; set; }
        float SeVolume { get; set; }
        float NoteSeVolume { get; set; }
    }
}