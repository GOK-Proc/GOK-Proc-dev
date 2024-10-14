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
            {"Bgm_tense", "Tention" },
            {"Se_warning", "Novel_SE_temp/SN1" },
            {"Se_footstep", "Novel_SE_temp/SN2" },
            {"Se_sword", "Novel_SE_temp/SN3" },
            {"Se_magic", "Novel_SE_temp/SN4" },
            {"Se_break", "Novel_SE_temp/SN5" },
            {"Se_power", "Novel_SE_temp/SN6" }
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
                // SEは再生が終わるまで待機
                NovelManager.Instance.IsProcessingSound = true;
                SEManager.Instance.Play("SE/" + _soundDict[SoundData.Sound], isLoop: false, callback: () => NovelManager.Instance.IsProcessingSound = false);
            }
            else if (prefix == "Stop")
            {
                switch (SoundData.Motion)
                {
                    case "Fade":
                        BGMManager.Instance.FadeOut(NovelManager.Instance.Duration);
                        break;

                    case "Cut":
                        BGMManager.Instance.Stop();
                        break;

                    default:
                        throw new Exception("BGMの変化方法が正しく指定されていません。");
                }
            }
            else
            {
                throw new Exception("BGMファイル名の接頭辞は\"Bgm_\"または\"Se_\"である必要があります。");
            }
        }
    }
}
