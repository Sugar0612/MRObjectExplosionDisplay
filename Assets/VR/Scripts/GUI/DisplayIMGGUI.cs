using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DisplayImgGUI : MonoBehaviour
{
    private List<VRDisplayGUIObject> displayGUIObjects = new List<VRDisplayGUIObject>();

    private VRDisplayGUIObject previousDisplayGuiObject = null;

    private static DisplayImgGUI instance;

    public static DisplayImgGUI Get()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<DisplayImgGUI>();
        }
        return instance;
    }

    private void Start()
    {
        displayGUIObjects = GetComponentsInChildren<VRDisplayGUIObject>().ToList();
        for (int i = 0; i < displayGUIObjects.Count; ++i)
        {
            int idx = i;
            displayGUIObjects[idx].OnClickGUIObjectEvent += OnClickGuiObject;
            displayGUIObjects[idx].OnRepositionEvent += CancelPreviousDisplayGuiObject;
        }

        SetAllGuiObjectActive(false, 0.0f);
    }

    private void OnClickGuiObject(VRDisplayGUIObject curr)
    {
        if (previousDisplayGuiObject != null)
        {
            previousDisplayGuiObject.Reposition();
        }

        previousDisplayGuiObject = curr;
    }

    private void CancelPreviousDisplayGuiObject()
    {
        StartCoroutine(CancelDisplayGuiObject());
    }

    private IEnumerator CancelDisplayGuiObject()
    {
        previousDisplayGuiObject = null;

        bool isFinished = true;
        foreach (var gui in displayGUIObjects)
        {
            isFinished &= gui.IsFinished;
        }

        yield return new WaitForSeconds(1.0f);

        if (isFinished)
        {
            SetAllGuiObjectActive(false, 1.0f);
            yield return new WaitForSeconds(1.0f);
            StepList.Get().Step2();
        }
    }


    public void SetAllGuiObjectActive(bool active, float duration)
    {
        foreach (var gui in displayGUIObjects)
        {
            gui.GradientSetActive(active, duration);
        }
    }

    public void SetAllGuiObjectActiveForImage(bool active, float duration)
    {
        foreach (var gui in displayGUIObjects)
        {
            float val = active ? 1.0f : 0.0f;
            gui.titleImg.DOFade(val, duration);
        }
    }

    public void SetAllGuiObjectActiveForText(bool active, float duration)
    {
        foreach (var gui in displayGUIObjects)
        {
            float val = active ? 1.0f : 0.0f;
            gui.hintText.DOFade(val, duration);
        }
    }
}
