using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Gallery;

namespace MusicSelection
{
    public abstract class ThumbnailBase : MonoBehaviour
    {
        [SerializeField] protected Image _image;
        [SerializeField] protected TextMeshProUGUI _titleText;
        [SerializeField] protected TextMeshProUGUI _composerText;

        public abstract void Set(TrackInformation trackInfo);
    }
}