using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldView : MonoBehaviour
{
    public Transform pointFieldCenter;
    public GameObject pCell;
    GameObject[,] cellObjects;

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

        for (int i=0; i < len; i++)
            for (int j=0; j < hgt; j++)
            {
                cellObjects[i, j] = Instantiate(pCell, transform);
                cellObjects[i, j].transform.position += new Vector3((i - (len / 2)) * cellSize, 0, ((hgt - 1 - j) - (hgt / 2)) * cellSize);

                if ((cells[i, j].cType == CellType.Furniture) && (!furnitureAlreadySpawnedHereFlags[i, j]))
                {
                    cellObjects[i, j].GetComponent<CellView>().SetCellView(cells[i, j], furnitureAlreadySpawnedHereFlags[i, j]);
                    //Debug.Log("for cell (" + i + "," + j + "filling non-spawn squares");
                    int fFormIdx = (int)cells[i, j].fType;
                    for (int k = i; k < i + furniture_forms[fFormIdx].GetLength(0); k++)
                        for (int m = j; m < j + furniture_forms[fFormIdx].GetLength(1); m++)
                            if (furniture_forms[fFormIdx][k - i, m - j])
                                furnitureAlreadySpawnedHereFlags[k, m] = true;
                }
                else
                    cellObjects[i, j].GetComponent<CellView>().ClearCell();
            }
    }
}
