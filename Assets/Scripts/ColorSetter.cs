using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSetter : MonoBehaviour
{
    public void SetColor(Color clr)
    {
        var renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
            renderer.material.SetColor("_Color", clr);
        else
        {
            var renderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer r in renderers)
                r.material.SetColor("_Color", clr);
        }
    }
}
