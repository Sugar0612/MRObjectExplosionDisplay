using DG.Tweening;
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

    [SerializeField] private float displayHeight = 1.6f;

    private Tween rotateTween;

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

    #region Collider Trigger Events

    private void StartDisplay(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            //displayObject.gameObject.SetActive(true);

            //ExplosionToolkit.Get().Load(displayObject.gameObject);
            //ExplosionToolkit.Get().Explosion();
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

        rotateTween = displayObject.transform.DORotate(new Vector3(0.0f, 360.0f, 0.0f), 7.0f, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Restart)
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

        displayObject.transform.DORotate(new Vector3(0.0f, 180.0f, 0.0f), 3.0f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => 
            {
                ExplosionToolkit.Get().Load(displayObject.gameObject);
                ExplosionToolkit.Get().Explosion();
            });
    }

    /// <summary>
    /// 算法部分
    /// </summary>
    public void PlayDisplayObjectAnimation()
    {
        ExplosionToolkit.Get().Recovery(() => displayAnimator.SetBool("isPlay", true));
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
}
