using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputDisabler : MonoBehaviour, ISubmitHandler, ICancelHandler
{
    [SerializeField] private bool _submit = true;
    [SerializeField] private bool _cancel = true;
    
    public void OnSubmit(BaseEventData _)
    {
        if(!_submit) return;
        DisableInput();
    }

    public void OnCancel(BaseEventData _)
    {
        if(!_cancel) return;
        DisableInput();        
    }

    private void DisableInput()
    {
        InputSystem.DisableAllEnabledActions();
    }
}