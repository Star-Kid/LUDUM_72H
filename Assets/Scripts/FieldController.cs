using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    public FieldView fView;
    public FieldModel fModel;
    
    void Start()
    {
        fView.SpawnGameField(fModel.cells, fModel.furnitureForms);
        fView.OnFurniturePlaced.AddListener(TryToPlaceNewFurnitureAndUpdateCurrentLevel);
    }

    public void SpawnRandomLevel()
    {
        fModel.LoadLevelDataRandom();
        fView.SpawnGameField(fModel.cells, fModel.furnitureForms);
    }

    public void SpawnFixedLevel(int level_idx)
    {
        fModel.LoadLevelDataFixed(level_idx);
        fView.SpawnGameField(fModel.cells, fModel.furnitureForms);
    }

    public void TryToPlaceNewFurnitureAndUpdateCurrentLevel(int x, int y, FurnitureType f_type, FurnitureColor f_color)
    {
        fModel.TryToPlaceNewFurnitureToCoords(x, y, f_type, f_color);
        fView.SpawnGameField(fModel.cells, fModel.furnitureForms);
    }
}
