using UnityEngine;
using UnityEngine.EventSystems;
using KanKikuchi.AudioManager;

public class SystemSoundEffect : MonoBehaviour, ISubmitHandler, ICancelHandler, IDeselectHandler
{
    [SerializeField] private bool _submit = true, _cancel = true, _deselect = true;

    public void OnSubmit(BaseEventData _)
    {
        if (!_submit) return;
        SEManager.Instance.Play(SEPath.SYSTEM_SUBMIT);
    }


    public void OnCancel(BaseEventData _)
    {
        if (!_cancel) return;
        SEManager.Instance.Play(SEPath.SYSTEM_CANCEL);
    }

    // SelectではなくDeselectのタイミング．
    // もしSelectにすると，Awake()やStart()のタイミングで
    // EventSystem.firstSelectedGameObjectを更新した時にも音が鳴ってしまうため． 
    public void OnDeselect(BaseEventData _)
    {
        if (!_deselect) return;
        SEManager.Instance.Play(SEPath.SYSTEM_SELECT);
    }
}