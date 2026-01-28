using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pico;
using Unity.XR.PXR;

public class MRMode : MonoBehaviour
{
    void Start()
    {
        PXR_MixedReality.EnableVideoSeeThroughEffect(true);
    }
}
