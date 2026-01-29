using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DisplayActionStruct
{
    [SerializeField]
    public AudioClip CommentaryMusic;

    [SerializeField]
    public UnityEvent Action;

    [SerializeField]
    public UnityEvent EndAction;
}
