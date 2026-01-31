using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayAction : MonoBehaviour
{
    private MeshColliderManager displayObject;

    private DisplayCollider displayCollider;

    private AudioSource displayAudioSource;

    private Animator displayAnimator;

    public List<DisplayActionStruct> ActionsList = new List<DisplayActionStruct>();

    private int currActionIndex = 0;

    private void Start()
    {
        displayObject = GetComponentInChildren<MeshColliderManager>();
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
        currActionIndex = 0;
        AudioManager.Get().UnLoad();
        displayAnimator.SetBool("isPlay", false);
    }

    public void ShowDisplayObject()
    {
        displayObject.gameObject.SetActive(true);
    }

    public void ExplosionDisplayObject()
    {
        ExplosionToolkit.Get().Load(displayObject.gameObject);
        ExplosionToolkit.Get().Explosion();
    }

    public void PlayDisplayObjectAnimation()
    {
        ExplosionToolkit.Get().Recovery(() => displayAnimator.SetBool("isPlay", true));
    }

    #endregion
}
