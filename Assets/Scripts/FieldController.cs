using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using System;

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

    public void SaveFieldToFile()
    {
        try
        {
            string dir_path = System.IO.Directory.GetCurrentDirectory();
            var extensions = new[] {
                new ExtensionFilter("Text files", "txt"),
                new ExtensionFilter("All files", "*" ),
            };
            var path = StandaloneFileBrowser.SaveFilePanel("Save File", dir_path, "level_" + DateTime.Now.ToString("yyyy.MM.dd_\\hhh\\mmm\\sss") + ".txt", extensions);
            if (path != "")
            {
                SaveFieldConfigToFile(path);
                Debug.Log("Level file saved: " + path);
            }
            else
                Debug.LogWarning("Filename is not selected!");
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Error during field file save (?): " + ex.Message);
        }
    }

    void SaveFieldConfigToFile(string filename)
    {
        string data = fModel.ExtractCurrentFieldRawData();
        try
        {
            System.IO.File.WriteAllText(filename, data);
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Error during field file save: " + ex.Message);
        }
    }
}
