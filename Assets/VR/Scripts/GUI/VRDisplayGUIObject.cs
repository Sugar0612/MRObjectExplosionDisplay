using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VRDisplayGUIObject : MonoBehaviour
{
    [SerializeField] private Vector3 displayPosition;

    [SerializeField] private Vector3 origiPosition;

    [SerializeField] private TextMeshProUGUI hintText;

    [SerializeField] private string hintMessage;

    [SerializeField] private float displayDuration = 15.0f;

    private Button vrDisplayButton;

    private AudioSource audioSource;

    private AudioClip clip;

    public bool canDisplay = true;

    public event Action OnClickGUIObject;

    private void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        vrDisplayButton = GetComponentInChildren<Button>();
        vrDisplayButton.onClick.AddListener(() => { OnClickGUIObject?.Invoke(); });
    }

    public void Display()
    {
        canDisplay = true;
        hintText.DOFade(1.0f, 0.5f).SetEase(Ease.InOutSine);
        transform.DOLocalMove(displayPosition, 1.0f);

        AudioManager.Get().Load(audioSource);
        AudioManager.Get().Play(clip);
    }

    public void Reposition()
    {
        canDisplay = false;
        hintText.DOFade(0.0f, 0.5f).SetEase(Ease.InOutSine);
        transform.DOLocalMove(origiPosition, 1.0f);
        AudioManager.Get().UnLoad();
    }

    public void AutoDisplay()
    {
        StartCoroutine(AutoDisplayCoroutine());
    }

    private IEnumerator AutoDisplayCoroutine()
    {
        Display();

        yield return new WaitUntil(() => AudioManager.Get().IsPlaying() == false || canDisplay == false);

        Reposition();
    }
}
