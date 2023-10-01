using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CellView : MonoBehaviour
{
    public SpriteRenderer sRend;
    public TMP_Text label;

    public void SetCellView(Cell cell_info)
    {
        switch (cell_info.cType)
        {
            case CellType.Empty:
                SetCellColor(Color.white);
                SetCellInfo("");
                break;
            case CellType.Exit:
                SetCellColor(Color.white);
                SetCellInfo("EXIT");
                break;
            case CellType.Furniture:
                switch (cell_info.fType)
                {
                    case FurnitureType.Refrigerator:
                        SetCellInfo("Refrigerator");
                        break;
                    case FurnitureType.Stove:
                        SetCellInfo("Stove");
                        break;
                    case FurnitureType.Toilet:
                        SetCellInfo("WC");
                        break;
                    case FurnitureType.Sofa:
                        SetCellInfo("Sofa");
                        break;
                    case FurnitureType.Type5:
                        SetCellInfo("Type 5");
                        break;
                    case FurnitureType.Type6:
                        SetCellInfo("Type 6");
                        break;
                }
                switch (cell_info.fColor)
                {
                    case FurnitureColor.Green:
                        SetCellColor(Color.green);
                        break;
                    case FurnitureColor.Orange:
                        SetCellColor(new Color(1, 0.5f, 0));
                        break;
                    case FurnitureColor.Red:
                        SetCellColor(Color.red);
                        break;
                    case FurnitureColor.Yellow:
                        SetCellColor(Color.yellow);
                        break;
                }
                break;
        }
    }

    void SetCellColor(Color color)
    {
        sRend.color = color;
    }

    void SetCellInfo(string info)
    {
        label.text = info;
    }
}
