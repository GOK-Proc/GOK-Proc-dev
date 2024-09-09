using KanKikuchi.AudioManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Novel
{
    public class SoundOperation : MonoBehaviour
    {
        public void UpdateSound(SoundData SoundData)
        {
            string prefix = SoundData.Sound.Split('_')[0];

            if (prefix == "Bgm")
            {
                if (!BGMManager.Instance.IsPlaying())
                {
                    BGMManager.Instance.Play("BGM/" + SoundData.Sound);
                }
                else
                {
                    switch (SoundData.Motion)
                    {
                        case "Fade":
                            BGMManager.Instance.FadeOut(NovelManager.Instance.Duration, () =>
                            {
                                BGMManager.Instance.Play("BGM/" + SoundData.Sound);
                            });
                            break;

                        case "Cut":
                            BGMManager.Instance.Stop();
                            BGMManager.Instance.Play("BGM/" + SoundData.Sound);
                            break;

                        default:
                            throw new Exception("BGMの変化方法が正しく指定されていません。");
                    }
                }
            }
            else if (prefix == "Se")
            {
                BGMManager.Instance.Play("BGM/" + SoundData.Sound, isLoop: false, allowsDuplicate: true);
            }
            else
            {
                throw new Exception("BGMファイル名の接頭辞は\"Bgm_\"または\"Se_\"である必要があります。");
            }
        }
    }
}
