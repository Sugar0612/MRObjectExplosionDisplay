using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class StepStruct
{
    [SerializeField]
    public AudioClip audioClip;

    [SerializeField]
    public UnityEvent Action;
}

public class StepList : MonoBehaviour
{
    public GameObject Star;

    public GameObject ChuanSong;

    public GameObject SuanPan;

    public GameObject YuanBao;

    public GameObject Lizi;

    [SerializeField] private AudioSource audioSource;

    public List<StepStruct> steps = new List<StepStruct>();

    private int currStepIndex = 0;

    private static StepList instance;

    public static StepList Get()
    {
        if (instance == null)
        {
            instance = FindAnyObjectByType<StepList>();
        }
        return instance;
    }

    private void Start()
    {
        Star.gameObject.SetActive(false);
        ChuanSong.gameObject.SetActive(false);
        Lizi.gameObject.SetActive(false);
        YuanBao.gameObject.SetActive(false);
        ChuanSong.gameObject.SetActive(false);
        SuanPan.gameObject.SetActive(false);
        Step1();
    }

    public void Step1()
    {
        StartCoroutine(Step1Coroutine());
    }

    private IEnumerator Step1Coroutine()
    {
        audioSource.clip = steps[currStepIndex].audioClip;
        audioSource.Play();

        ChuanSong.gameObject.SetActive(true);

        yield return new WaitUntil(() => audioSource.isPlaying == false);

        Star.gameObject.SetActive(true);
        ChuanSong.gameObject.SetActive(false);
        DisplayImgGUI.Get().SetAllGuiObjectActiveForImage(true, 1.0f);
    }

    public void Step2()
    {
        StartCoroutine(Stpe2Coroutine());
    }

    private IEnumerator Stpe2Coroutine()
    {
        currStepIndex++;
        audioSource.clip = steps[currStepIndex].audioClip;
        audioSource.Play();

        yield return new WaitUntil(() => audioSource.isPlaying == false);

        Step3();
    }

    public void Step3()
    {
        StartCoroutine(Stpe3Coroutine());
    }

    private IEnumerator Stpe3Coroutine()
    {
        currStepIndex++;
        audioSource.clip = steps[currStepIndex].audioClip;
        audioSource.Play();

        SuanPan.gameObject.SetActive(true);
        Lizi.gameObject.SetActive(true);
        YuanBao.gameObject.SetActive(true);

        Renderer renderer = YuanBao.GetComponentInChildren<Renderer>();

        float val = 10.0f;
        while (true)
        {
            val -= 0.01f;
            renderer.material.SetFloat("_jianbian", val);
            yield return new WaitForSeconds(0.05f);
            if (val < 0.0f) break;
        }

        yield return null;
    }
}
