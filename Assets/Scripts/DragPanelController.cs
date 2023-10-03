using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragPanelController : MonoBehaviour
{
    public GameObject pFurnitureForDrag;
    public GameObject pExitForDrag;
    public Transform dragPanelContent;
    public FieldModel fModel;
    GameObject[,] furniturePanels;

    void Start()
    {
        FillPanelByFurnitureElements();
    }

    public void FillPanelByFurnitureElements()
    {
        Instantiate(pExitForDrag, dragPanelContent);

        furniturePanels = new GameObject[fModel.numberOfFurnitures, fModel.numberOfColors];

        for (int j = 0; j < fModel.numberOfColors; j++)
            for (int i = 0; i < fModel.numberOfFurnitures; i++)
            {
                furniturePanels[i, j] = Instantiate(pFurnitureForDrag, dragPanelContent);
                furniturePanels[i, j].GetComponent<PanelFurnitureForDragController>().SetView((FurnitureType)i, (FurnitureColor)j);
            }
    }
}
