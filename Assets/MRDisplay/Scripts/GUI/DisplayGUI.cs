using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class DisplayGUIStruct
{
    [SerializeField] public string Title;

    [SerializeField] public string Explain;

    [SerializeField] public int Duration;
}

public class DisplayGUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;

    [SerializeField] private TextMeshProUGUI explainText;

    private void Start()
    {
        FadeOut();
    }

    /// <summary>
    /// œ˚ ß
    /// </summary>
    public void FadeOut()
    {
        titleText.DOFade(0f, 0.5f).SetEase(Ease.InOutSine);
        explainText.DOFade(0f, 0.5f).SetEase(Ease.InOutSine);
    }

    /// <summary>
    /// œ‘ æ
    /// </summary>
    public void FadeIn()
    {
        titleText.DOFade(1.0f, 0.5f).SetEase(Ease.InOutSine);
        explainText.DOFade(1.0f, 0.5f).SetEase(Ease.InOutSine);
    }

    public void AutoFadeInAndOut(DisplayGUIStruct guiData)
    {
        if (guiData != null)
        {
            SetTitle(guiData.Title);
            SetExplain(guiData.Explain);
            StartCoroutine(AutoFadeInAndOutCoroutine(guiData.Duration));
        }
    }

    private IEnumerator AutoFadeInAndOutCoroutine(float duration)
    {
        FadeIn();

        yield return new WaitForSeconds(duration);

        FadeOut();
    }

    public void SetTitle(string title)
    {
        titleText.SetText(VerticalTextProcessing(title));
    }

    public void SetExplain(string explain)
    {
        explainText.SetText(explain);
    }

    private string VerticalTextProcessing(string text)
    {
        string verText = "";
        for (int i = 0; i < text.Length; i++)
        {
            verText += text[i] + "\n";
        }

        return verText;
    }
}
