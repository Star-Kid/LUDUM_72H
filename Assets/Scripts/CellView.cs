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
                SetCellColor(Color.cyan);
                SetCellInfo("EXIT");
                break;
            case CellType.Furniture:
                if (!furniture_already_spawned)
                {
                    GameObject currentFurniture = Instantiate(furnitures[(int)cell_info.fType], transform);
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

    public void OnMouseOver()
    {
        FieldController fc = transform.parent.GetComponent<FieldController>();
        if (fc.isEditorMode)
            if (Input.GetMouseButtonDown(1)) //right click
            {
                //Debug.Log("right click on cell in editor mode!");
                transform.parent.GetComponent<FieldView>().GetRightClickCoors(gameObject);
            }
    }
}
