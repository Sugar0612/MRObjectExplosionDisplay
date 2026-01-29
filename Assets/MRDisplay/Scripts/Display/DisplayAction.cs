using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayAction : MonoBehaviour
{
    [SerializeField] private MeshColliderManager displayObject;

    [SerializeField] private DisplayCollider displayCollider;

    public List<DisplayActionStruct> ActionsList = new List<DisplayActionStruct>();

    private int currActionIndex = 0;

    private void Start()
    {
        displayObject = GetComponentInChildren<MeshColliderManager>();
        displayCollider = GetComponentInChildren<DisplayCollider>();
        displayCollider.TriggerEnterEvent += StartDisplay;
        displayCollider.TriggerExitEvent += ExitDisplay;

        displayObject.gameObject.SetActive(false);
    }

    #region Collider Trigger Events

    private void StartDisplay(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            DisplayAudio.Get().Load(GetComponent<AudioSource>());
            displayObject.gameObject.SetActive(true);

            ExplosionToolkit.Get().Load(displayObject.gameObject);
            ExplosionToolkit.Get().Explosion();
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

    public void NextAction()
    {
        currActionIndex++;
    }

    #endregion
}
