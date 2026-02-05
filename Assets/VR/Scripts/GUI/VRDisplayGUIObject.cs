using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class VRDisplayGUIObject : MonoBehaviour
{
    [SerializeField] private Vector3 displayPosition;

    [SerializeField] private Vector3 origiPosition;

    [SerializeField] private string hintMessage;

    [SerializeField] private float displayDuration = 15.0f;

    public TextMeshProUGUI hintText;

    public Image titleImg;

    private Button vrDisplayButton;

    private AudioSource audioSource;

    private Sequence floatSequence;

    public bool CanDisplay = true;

    public event Action<VRDisplayGUIObject> OnClickGUIObjectEvent;

    public event Action OnRepositionEvent;

    [SerializeField] private AudioClip clip;

    [SerializeField] private float floatHeight = 0.5f;  // 漂浮高度

    [SerializeField] private float floatDuration = 2f;  // 单次漂浮时间

    public bool IsFinished = false;

    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        hintText = GetComponentInChildren<TextMeshProUGUI>();
        titleImg = GetComponentInChildren<Image>();
        vrDisplayButton = GetComponentInChildren<Button>();
    }

    private void Start()
    {
        StartFloating();

        vrDisplayButton.onClick.AddListener(() => 
        {
            OnClickGUIObjectEvent?.Invoke(this);
            AutoDisplay();
        });
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StopAllCoroutines();
        }
    }

    private void Display()
    {
        CanDisplay = true;
        floatSequence.Kill();

        hintText.DOFade(1.0f, 0.5f).SetEase(Ease.InOutSine);
        transform.DOLocalMove(displayPosition, 1.0f);

        AudioManager.Get().Load(audioSource);
        AudioManager.Get().Play(clip);

        IsFinished = true;
    }

    public void Reposition() 
    {
        if (CanDisplay)
        {
            StartCoroutine(RepositionCoroutine());
        }
    }

    private IEnumerator RepositionCoroutine()
    {
        OnRepositionEvent?.Invoke();
        CanDisplay = false;
        hintText.DOFade(0.0f, 0.5f).SetEase(Ease.InOutSine);
        transform.DOLocalMove(origiPosition, 1.0f);
        AudioManager.Get().UnLoad();

        yield return new WaitForSeconds(1.0f);

        StartFloating();
        StopAllCoroutines();
    }

    public void AutoDisplay()
    {
        StartCoroutine(AutoDisplayCoroutine());
    }

    private IEnumerator AutoDisplayCoroutine()
    {
        Display();

        yield return new WaitUntil(() => AudioManager.Get().IsPlaying() == false);
        //yield return new WaitForSeconds(displayDuration);

        Reposition();
    }

    void StartFloating()
    {
        int randomValue = UnityEngine.Random.Range(0, 2) * 2 - 1;
        floatHeight = UnityEngine.Random.Range(0.1f, 0.3f);
        floatHeight = randomValue * floatHeight;

        // 保存初始位置
        Vector3 startPos = transform.position;

        // 创建上下漂浮序列
        floatSequence = DOTween.Sequence();

        // 向上移动
        floatSequence.Append(transform.DOMoveY(startPos.y + floatHeight, floatDuration)
            .SetEase(Ease.InOutSine));

        // 向下移动
        floatSequence.Append(transform.DOMoveY(startPos.y, floatDuration)
            .SetEase(Ease.InOutSine));

        // 设置循环（-1为无限循环）
        floatSequence.SetLoops(-1, LoopType.Restart);
    }

    public void GradientSetActive(bool active, float duration)
    {
        float val = active ? 1.0f : 0.0f;
        titleImg.DOFade(val, duration).SetEase(Ease.InOutSine);
        hintText.DOFade(val, duration).SetEase(Ease.InOutSine);
    }
}
