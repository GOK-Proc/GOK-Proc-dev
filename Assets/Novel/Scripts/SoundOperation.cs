using KanKikuchi.AudioManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Novel
{
    public class SoundOperation : MonoBehaviour
    {
        private Dictionary<string, string> _soundDict = new Dictionary<string, string>()
        {
            {"Bgm_village", "Town" },
            {"Bgm_forest", "Forest" },
            {"Se_warning", "Novel_SE_temp/Se_temp" },
            {"Se_footstep", "Novel_SE_temp/Se_temp" },
            {"Se_sword", "Novel_SE_temp/Se_temp" },
            {"Se_magic", "Novel_SE_temp/Se_temp" },
            {"Se_break", "Novel_SE_temp/Se_temp" }
        };

        public void UpdateSound(SoundData SoundData)
        {
            string prefix = SoundData.Sound.Split('_')[0];

            if (prefix == "Bgm")
            {
                if (!BGMManager.Instance.IsPlaying())
                {
                    BGMManager.Instance.Play("BGM/" + _soundDict[SoundData.Sound]);
                }
                else
                {
                    switch (SoundData.Motion)
                    {
                        case "Fade":
                            BGMManager.Instance.FadeOut(NovelManager.Instance.Duration, () =>
                            {
                                BGMManager.Instance.Play("BGM/" + _soundDict[SoundData.Sound]);
                            });
                            break;

                        case "Cut":
                            BGMManager.Instance.Stop();
                            BGMManager.Instance.Play("BGM/" + _soundDict[SoundData.Sound]);
                            break;

                        default:
                            throw new Exception("BGMの変化方法が正しく指定されていません。");
                    }
                }
            }
            else if (prefix == "Se")
            {
                BGMManager.Instance.Play("BGM/" + _soundDict[SoundData.Sound], isLoop: false, allowsDuplicate: true);
            }
            else
            {
                throw new Exception("BGMファイル名の接頭辞は\"Bgm_\"または\"Se_\"である必要があります。");
            }
        }
    }
}
