using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using KanKikuchi.AudioManager;

/// <summary>
/// UIに関連するEventを発生させるEventTriggerをコンポーネントに持つゲームオブジェクトにアタッチすることで，
/// システムSEを再生するイベントを自動で登録する．
/// </summary>
[RequireComponent(typeof(EventTrigger))]
public class SystemSoundEffect : MonoBehaviour
{
    private EventTrigger _trigger;

    private void Awake()
    {
        _trigger = GetComponent<EventTrigger>();
    }

    private void Start()
    {
        var triggerTypes = new[]
        {
            EventTriggerType.Submit,
            EventTriggerType.Cancel,
            // SelectではなくDeselectのタイミング．
            // もしSelectにすると，Awake()やStart()のタイミングで
            // EventSystem.firstSelectedGameObjectを更新した時にも音が鳴ってしまうため．
            EventTriggerType.Deselect
        };

        foreach (var triggerType in triggerTypes)
        {
            EventTrigger.Entry entry = new() { eventID = triggerType };

            var sePath = triggerType switch
            {
                EventTriggerType.Submit => SEPath.SYSTEM_SUBMIT,
                EventTriggerType.Cancel => SEPath.SYSTEM_CANCEL,
                EventTriggerType.Deselect => SEPath.SYSTEM_SELECT,
                _ => null
            };
            entry.callback.AddListener((_) => SEManager.Instance.Play(sePath));

            // DeSelectには必ず再生イベントを登録する．
            if (triggerType == EventTriggerType.Deselect)
            {
                _trigger.triggers.Add(entry);
                continue;
            }

            // すでにエントリが登録されているタイプのトリガーにのみ再生イベントを登録する．
            // これがないと，特定の場面ではボタンを押しても何も起きないのに音だけ鳴ってしまう．
            // 例：タイトルでのCancel，ギャラリーでのSubmit
            if (_trigger.triggers.Any(e => e.eventID == triggerType))
            {
                _trigger.triggers.Add(entry);
            }
        }
    }
}