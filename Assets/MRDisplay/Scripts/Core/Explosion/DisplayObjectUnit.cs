using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class DisplayObjectUnit : MonoBehaviour
{
    public bool isTrigger;
    public bool castShadow;
    public bool recieveShadow;

    List<MeshRenderer> renderers = new List<MeshRenderer>();

    void Awake()
    {
        GetComponentsInChildren(renderers);
        MeshCollider meshCollider;
        foreach (MeshRenderer renderer in renderers)
        {
            meshCollider = renderer.gameObject.AddComponent<MeshCollider>();
            meshCollider.convex = isTrigger;
            meshCollider.isTrigger = isTrigger;
            renderer.receiveShadows = recieveShadow;
            renderer.shadowCastingMode = castShadow ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }
}
