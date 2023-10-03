using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSetter : MonoBehaviour
{
    public void SetColor(FurnitureColor f_color)
    {
        Color clr = ConverterUtils.GetColorFromFurnitureColor(f_color);

        var renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
            renderer.material.SetColor("_Color", clr);
        else
        {
            var renderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer r in renderers)
            {
                if (r.material.name.Contains("Default-Material"))
                    r.material.SetColor("_Color", clr);
                else
                    if (r.material.name.Contains("colour_01") || r.material.name.Contains("_color_changable"))
                        r.material.SetColor("_Color", clr);
            }
        }
    }
}
