using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCollider : MonoBehaviour
{
    public delegate void OnTriggerEvent(Collider other);

    public event OnTriggerEvent TriggerEnterEvent;

    public event OnTriggerEvent TriggerExitEvent;

    public void OnTriggerEnter(Collider other)
    {
        TriggerEnterEvent?.Invoke(other);
    }

    public void OnTriggerExit(Collider other)
    {
        TriggerExitEvent?.Invoke(other);
    }
}
