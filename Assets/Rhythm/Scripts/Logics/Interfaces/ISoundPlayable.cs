namespace Rhythm
{
    public interface ISoundPlayable
    {
        void PlayMusic();
        void StopMusic();
        void PauseMusic();
        void UnPauseMusic();
        void FadeOutMusic(float duration);
        void PlaySE(string id, float delay = 0f);
        void PlaySE(string id, int index, float delay = 0f);
        void StopSE(string id, int index);
        void FadeOutSE(string id, int index, float duration);
        void PlayIntroSE(string id, float delay = 0f);
        void StopIntroSE(string id);
        void FadeOutIntroSE(string id, float duration);
    }
}