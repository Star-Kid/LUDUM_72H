using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FieldView : MonoBehaviour
{
    public Transform pointFieldCenter;
    public GameObject pCell;
    GameObject[,] cellObjects;

    [HideInInspector]
    public UnityEvent<int, int, FurnitureType, FurnitureColor> OnFurniturePlaced;
    public UnityEvent<int, int> OnCellClear;

    public void SpawnGameField(Cell[,] cells, bool[][,] furniture_forms)
    {
        if (cellObjects != null)
            foreach (GameObject cellObj in cellObjects)
                Destroy(cellObj);

        int len = cells.GetLength(0);
        int hgt = cells.GetLength(1);
        const float cellSize = 0.27f;
        cellObjects = new GameObject[len, hgt];

        bool[,] furnitureAlreadySpawnedHereFlags = new bool[len, hgt];

        for (int i = 0; i < len; i++)
            for (int j = 0; j < hgt; j++)
            {
                cellObjects[i, j] = Instantiate(pCell, transform);
                cellObjects[i, j].transform.position += new Vector3((i - (len / 2)) * cellSize, 0, ((hgt - 1 - j) - (hgt / 2)) * cellSize);

                if (cells[i, j].cType == CellType.Furniture)
                { 
                    if (!furnitureAlreadySpawnedHereFlags[i, j])
                    {
                        cellObjects[i, j].GetComponent<CellView>().SetCellView(cells[i, j], furnitureAlreadySpawnedHereFlags[i, j]);
                        //Debug.Log("for cell (" + i + "," + j + "filling non-spawn squares");
                        int fFormIdx = (int)cells[i, j].fType;
                        for (int k = i; k < i + furniture_forms[fFormIdx].GetLength(0); k++)
                            for (int m = j; m < j + furniture_forms[fFormIdx].GetLength(1); m++)
                                if (furniture_forms[fFormIdx][k - i, m - j])
                                    furnitureAlreadySpawnedHereFlags[k, m] = true;
                    }
                        cellObjects[i, j].GetComponent<CellView>().ClearCell();
                }
                else
                    cellObjects[i, j].GetComponent<CellView>().SetCellView(cells[i, j], furnitureAlreadySpawnedHereFlags[i, j]);
            }
    }

    public void GetDragEndHitCoors(GameObject hit_object, FurnitureType f_type, FurnitureColor f_color)
    {
        for (int i = 0; i < cellObjects.GetLength(0); i++)
            for (int j = 0; j < cellObjects.GetLength(1); j++)
                if (cellObjects[i, j].Equals(hit_object))
                {
                    //Debug.Log("Hit coords: " + i + "," + j);
                    OnFurniturePlaced.Invoke(i, j, f_type, f_color);
                    return;
                }
        Debug.LogWarning("There is no cell object that match hit object!");
    }

    public void GetRightClickCoors(GameObject hit_object)
    {
        for (int i = 0; i < cellObjects.GetLength(0); i++)
            for (int j = 0; j < cellObjects.GetLength(1); j++)
                if (cellObjects[i, j].Equals(hit_object))
                {
                    //Debug.Log("Right click in editor mode coords: " + i + "," + j);
                    OnCellClear.Invoke(i, j);
                    return;
                }
        Debug.LogWarning("There is no cell object that match right click object!");
    }
}
