using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSetter : MonoBehaviour
{
    public void SetColor(FurnitureColor f_color)
    {
        Color clr = Color.magenta;
        switch (f_color)
        {
            case FurnitureColor.Green:
                clr = Color.green;
                break;
            case FurnitureColor.Orange:
                clr = new Color(1, 0.5f, 0);
                break;
            case FurnitureColor.Red:
                clr = Color.red;
                break;
            case FurnitureColor.Yellow:
                clr = Color.yellow;
                break;
        }

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
