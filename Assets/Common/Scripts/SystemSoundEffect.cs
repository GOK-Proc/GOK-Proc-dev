using UnityEngine;
using UnityEngine.EventSystems;
using KanKikuchi.AudioManager;

public class SystemSoundEffect : MonoBehaviour, ISubmitHandler, ICancelHandler, IDeselectHandler
{
    private const float VolumeRate = 0.2f;
    [SerializeField] private bool _submit = true, _cancel = true, _deselect = true;

    public static void PlaySubmit()
    {
        SEManager.Instance.Play(SEPath.SYSTEM_SUBMIT, VolumeRate);
    }

    public static void PlayCancel()
    {
        SEManager.Instance.Play(SEPath.SYSTEM_CANCEL, VolumeRate);
    }

    public static void PlaySelect()
    {
        SEManager.Instance.Play(SEPath.SYSTEM_SELECT, VolumeRate);
    }

    public void OnSubmit(BaseEventData _)
    {
        if (!_submit) return;
        PlaySubmit();
    }


    public void OnCancel(BaseEventData _)
    {
        if (!_cancel) return;
        PlayCancel();
    }

    // SelectではなくDeselectのタイミング．
    // もしSelectにすると，Awake()やStart()のタイミングで
    // EventSystem.firstSelectedGameObjectを更新した時にも音が鳴ってしまうため． 
    public void OnDeselect(BaseEventData _)
    {
        if (!_deselect) return;
        PlaySelect();
    }
}