using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldView : MonoBehaviour
{
    public Transform pointFieldCenter;
    public GameObject pCell;
    GameObject[,] cellObjects;

    public void SpawnGameField(Cell[,] cells)
    {
        if (cellObjects != null)
            foreach (GameObject cellObj in cellObjects)
                Destroy(cellObj);

        int len = cells.GetLength(0);
        int hgt = cells.GetLength(1);
        const float cellSize = 1.2f;
        cellObjects = new GameObject[len, hgt];
        for (int i=0; i < len; i++)
            for (int j=0; j < hgt; j++)
            {
                cellObjects[i, j] = Instantiate(pCell, transform);
                cellObjects[i, j].transform.position += new Vector3((i - (len / 2)) * cellSize, ((hgt -1 - j) - (hgt / 2)) * cellSize);
                cellObjects[i, j].GetComponent<CellView>().SetCellView(cells[i,j]);
            }
    }
}