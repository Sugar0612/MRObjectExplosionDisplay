using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DisplayCollider;

public class DisplayCollider : MonoBehaviour
{
    public enum ActionState
    {
        Wait, Playing, None
    }

    public ActionState ActionState_ = ActionState.None;

    public delegate void OnTriggerEvent(Collider other);

    public event OnTriggerEvent TriggerEnterEvent;

    public event OnTriggerEvent TriggerExitEvent;

    private void Start()
    {
        ActionState_ = ActionState.Wait;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (ActionState_ == ActionState.Wait)
        {
            ActionState_ = ActionState.Playing;
            TriggerEnterEvent?.Invoke(other);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //TriggerExitEvent?.Invoke(other);
    }
}
