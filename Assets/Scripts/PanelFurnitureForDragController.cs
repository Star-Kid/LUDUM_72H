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

        switch (f_type)
        {
            case FurnitureType.Refrigerator:
                label.text = "Refrigerator";
                break;
            case FurnitureType.Stove:
                label.text = "Stove";
                break;
            case FurnitureType.Toilet:
                label.text = "WC";
                break;
            case FurnitureType.Sofa:
                label.text = "Sofa";
                break;
            case FurnitureType.Type5:
                label.text = "Type 5";
                break;
            case FurnitureType.Type6:
                label.text = "Type 6";
                break;
        }

        switch (f_color)
        {
            case FurnitureColor.Green:
                GetComponent<Image>().color = Color.green;
                break;
            case FurnitureColor.Orange:
                GetComponent<Image>().color = new Color(1, 0.5f, 0);
                break;
            case FurnitureColor.Red:
                GetComponent<Image>().color = Color.red;
                break;
            case FurnitureColor.Yellow:
                GetComponent<Image>().color = Color.yellow;
                break;
        }
    }
}
