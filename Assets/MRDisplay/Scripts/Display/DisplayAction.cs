using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayAction : MonoBehaviour
{
    private DisplayObjectUnit displayObject;

    private DisplayCollider displayCollider;

    private AudioSource displayAudioSource;

    private Animator displayAnimator;

    public List<DisplayActionStruct> ActionsList = new List<DisplayActionStruct>();

    private int currActionIndex = 0;

    private int currPartsIndex = 0;

    #region Display Object Action Params

    [SerializeField] private float displayHeight = 1.6f;

    [SerializeField] private float explosionHeight = 1.6f;

    [SerializeField] private Vector3 defaultRotation = Vector3.zero;

    // 爆炸图阶段rotation
    [SerializeField] private Vector3 explosionRotation = Vector3.zero;

    [Serializable] private class DisplayObjectPartArray { [SerializeField] public List<DisplayObjectPart> groups = new List<DisplayObjectPart>(); }

    [SerializeField] private List<DisplayObjectPartArray> parts = new List<DisplayObjectPartArray>();

    private Tween rotateTween;

    #endregion

    private void Start()
    {
        displayObject = GetComponentInChildren<DisplayObjectUnit>();
        displayCollider = GetComponentInChildren<DisplayCollider>();
        displayAudioSource = GetComponent<AudioSource>();
        displayAnimator = GetComponentInChildren<Animator>();

        displayCollider.TriggerEnterEvent += StartDisplay;
        displayCollider.TriggerExitEvent += ExitDisplay;

        displayObject.gameObject.SetActive(false);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    ActionsList[currActionIndex]?.EndAction?.Invoke();
        //    //DisplayActionActuator.Get().Execute(ActionsList[currActionIndex]);
        //}
    }

    #region Collider Trigger Events

    private void StartDisplay(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            AudioManager.Get().Load(displayAudioSource);
            DisplayActionActuator.Get().Execute(ActionsList[currActionIndex]);
        }
    }

    private void ExitDisplay(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            ExplosionToolkit.Get().UnLoad();
        }
    }

    #endregion

    #region Display Methods

    public void EndAction()
    {
        currActionIndex++;
        if (currActionIndex >= ActionsList.Count)
        {
            ShutDownAction();
            return;
        }

        DisplayActionActuator.Get().Execute(ActionsList[currActionIndex]);
    }

    private void ShutDownAction()
    {
        rotateTween?.Kill();

        displayObject.transform.DORotate(Vector3.zero, 3.0f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                Vector3 displaylastPostion = displayObject.transform.position;
                displayObject.transform.DOMove(new Vector3(displaylastPostion.x, 0.0f, displaylastPostion.z), 2.0f)
                .OnComplete(() => 
                {
                    currActionIndex = 0;
                    AudioManager.Get().UnLoad();
                    displayAnimator.SetBool("isPlay", false);
                    displayObject.gameObject.SetActive(false);
                });
            });
    }

    /// <summary>
    /// 开头部分
    /// </summary>
    public void ShowDisplayObject()
    {
        displayObject.gameObject.SetActive(true);
        StartCoroutine(DisplayActionCoroutine());
    }

    private IEnumerator DisplayActionCoroutine()
    {
        Vector3 displaylastPostion = displayObject.transform.position;
        displayObject.transform.DOMove(new Vector3(displaylastPostion.x, displayHeight, displaylastPostion.z), 2.0f);

        yield return new WaitForSeconds(2.1f);

        rotateTween = displayObject.transform.DORotate(defaultRotation, 7.0f)
            .SetEase(Ease.Linear);
    }

    /// <summary>
    /// 结构部分
    /// </summary>
    public void ExplosionDisplayObject()
    {
        StartCoroutine(ExplosionActionCoroutine());
    }

    private IEnumerator ExplosionActionCoroutine()
    {
        rotateTween?.Kill();
        yield return null;

        Vector3 displaylastPostion = displayObject.transform.position;
        displayObject.transform.DOMove(new Vector3(displaylastPostion.x, explosionHeight, displaylastPostion.z), 1.0f)
            .OnComplete(() =>
            {
                displayObject.transform.DORotate(explosionRotation, 2.0f)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        Action showDisplayPart = () => ShowDisplayObjectPart();
                        ExplosionToolkit.Get().Load(displayObject.gameObject);
                        ExplosionToolkit.Get().Explosion(showDisplayPart);
                    });
            });
    }

    public void ExplosionEndAction()
    {
        //Debug.Log("ExplosionEndAction...");
        StartCoroutine(ExplosionEndActionCoroutine());
    }

    private IEnumerator ExplosionEndActionCoroutine()
    {
        displayObject.transform.DORotate(defaultRotation, 1.5f)
           .SetEase(Ease.OutQuad)
           .OnComplete(() =>
           {
               ExplosionToolkit.Get().Recovery();
           });

        yield return new WaitForSeconds(4.0f);

        //Debug.Log("next action...");
        currActionIndex++;
        if (currActionIndex >= ActionsList.Count)
        {
            ShutDownAction();
            yield break;
        }

        DisplayActionActuator.Get().Execute(ActionsList[currActionIndex]);
    }

    /// <summary>
    /// 算法部分
    /// </summary>
    public void PlayDisplayObjectAnimation()
    {
        displayAnimator.SetBool("isPlay", true);
    }

    /// <summary>
    /// 结尾部分
    /// </summary>
    public void Ending()
    {
        displayAnimator.SetBool("isPlay", false);
        rotateTween = displayObject.transform.DORotate(new Vector3(0.0f, 360.0f, 0.0f), 7.0f, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }

    #endregion

    public void ShowDisplayObjectPart()
    {
        if (currPartsIndex >= parts.Count) return;

        for (int i = 0; i < parts[currPartsIndex].groups.Count; i++)
        {
            //Debug.Log($"groups index: {i}");
            int idx = i;
            parts[currPartsIndex].groups[i].AutoDisplay(() =>
            {
                //Debug.Log($"idx: {idx}");
                if (idx == 0)
                {
                    currPartsIndex++;
                    ShowDisplayObjectPart();
                }
            });
        }
    }
}
