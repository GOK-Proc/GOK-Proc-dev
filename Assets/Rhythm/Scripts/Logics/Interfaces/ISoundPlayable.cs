namespace Rhythm
{
    public interface ISoundPlayable
    {
        void PlayMusic();
        void StopMusic();
        void PauseMusic();
        void UnPauseMusic();
        void FadeOutMusic();
        void PlaySE(string id);
    }
}