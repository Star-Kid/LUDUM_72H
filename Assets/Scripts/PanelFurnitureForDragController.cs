using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelFurnitureForDragController : MonoBehaviour
{
    public TMP_Text label;

    public FurnitureType fType { private set; get; }
    public FurnitureColor fColor { private set; get; }

    public void SetView(FurnitureType f_type, FurnitureColor f_color)
    {
        fType = f_type;
        fColor = f_color;

        label.text = f_type.ToString();
        GetComponent<Image>().color = ColorConverter.GetColorFromFurnitureColor(f_color);
    }
}
