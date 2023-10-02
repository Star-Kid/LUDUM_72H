using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    public FieldView fView;
    public FieldModel fModel;
    public bool isEditorMode { private set; get; }
    
    void Start()
    {
        fView.SpawnGameField(fModel.cells, fModel.furnitureForms);
        fView.OnFurniturePlaced.AddListener(TryToPlaceNewFurnitureAndUpdateCurrentLevel);
        fView.OnCellClear.AddListener(DeleteNonEmptyCellAndUpdateCurrentLevel);
        isEditorMode = false;
    }

    public void SpawnRandomLevel()
    {
        fModel.LoadLevelDataRandom();
        fView.SpawnGameField(fModel.cells, fModel.furnitureForms);
        isEditorMode = false;
    }

    public void SpawnFixedLevel(int level_idx)
    {
        fModel.LoadLevelDataFixed(level_idx);
        fView.SpawnGameField(fModel.cells, fModel.furnitureForms);
        isEditorMode = false;
    }

    public void SpawnEditorLevel(int width, int height)
    {
        fModel.LoadLevelDataEditor(width, height);
        fView.SpawnGameField(fModel.cells, fModel.furnitureForms);
        isEditorMode = true;
    }

    public void TryToPlaceNewFurnitureAndUpdateCurrentLevel(int x, int y, FurnitureType f_type, FurnitureColor f_color)
    {
        fModel.TryToPlaceNewFurnitureToCoords(x, y, f_type, f_color);
        fView.SpawnGameField(fModel.cells, fModel.furnitureForms);
    }

    public void DeleteNonEmptyCellAndUpdateCurrentLevel(int x, int y)
    {
        if (fModel.ClearCellOrFurnitureCellsIfNotEmpty(x, y))
            fView.SpawnGameField(fModel.cells, fModel.furnitureForms);
    }

}
