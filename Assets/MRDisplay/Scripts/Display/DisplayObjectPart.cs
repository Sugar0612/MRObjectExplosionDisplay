using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayObjectPart : MonoBehaviour
{
    [SerializeField] private Vector3 displayRotation;

    [SerializeField] private Vector3 displayPosition;

    [SerializeField] private Vector3 originalRotation;

    [SerializeField] private Vector3 originalPosition;

    [SerializeField] private float duration;

    /// <summary>
    /// 展示
    /// </summary>
    public void Display()
    {
        Tweener tweener = transform.DOLocalMove(displayPosition, 1.0f)
            .OnComplete(() => 
            {
                transform.DOLocalRotate(displayRotation, 1.0f)
                    .SetEase(Ease.OutQuad);
            });
    }

    /// <summary>
    /// 复位
    /// </summary>
    public void Reposition()
    {
        //Debug.Log("Reposition");
        transform.DOLocalMove(originalPosition, 1.0f);
        transform.DOLocalRotate(originalRotation, 1.0f);
    }

    /// <summary>
    /// 自动展示和复位
    /// </summary>
    public void AutoDisplay(Action callback)
    {
        StartCoroutine(AutoDisplayCoroutine(callback));
    }

    private IEnumerator AutoDisplayCoroutine(Action callback)
    {
        Display();

        yield return new WaitForSeconds(duration);

        Reposition();
        callback?.Invoke();
    }
}
