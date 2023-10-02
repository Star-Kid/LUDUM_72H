using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CellView : MonoBehaviour
{
    public SpriteRenderer sRend;
    public TMP_Text label;
    public GameObject[] furnitures;

    public void SetCellView(Cell cell_info, bool furniture_already_spawned)
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
                //SetCellInfo("");

                if (!furniture_already_spawned)
                {
                    GameObject currentFurniture = Instantiate(furnitures[(int)cell_info.fType], transform);
                    /*switch (cell_info.fType)
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
                    }*/
                    ColorSetter furnitureColorSetter = currentFurniture.GetComponent<ColorSetter>();
                    furnitureColorSetter.SetColor(cell_info.fColor);
                }
                break;
        }
    }

    public void ClearCell()
    {
        SetCellColor(Color.white);
        SetCellInfo("");
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
