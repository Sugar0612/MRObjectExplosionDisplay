using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayActionActuator : MonoBehaviour
{
    private static DisplayActionActuator instance;

    public static DisplayActionActuator Get()
    {
        if (instance == null)
        {
            instance = FindAnyObjectByType<DisplayActionActuator>();
        }
        return instance;
    }

    public void Execute(DisplayActionStruct displayNote)
    {
        displayNote.Action?.Invoke();
        StartCoroutine(ExcuteCoroutine(displayNote));
    }

    private IEnumerator ExcuteCoroutine(DisplayActionStruct displayNote)
    {
        yield return null;
    }
}
