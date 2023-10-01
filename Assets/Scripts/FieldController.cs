using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    public FieldView fView;
    public FieldModel fModel;
    
    void Start()
    {
        fView.SpawnGameField(fModel.cells);
    }

    public void SpawnRandomLevel()
    {
        fModel.LoadLevelDataRandom();
        fView.SpawnGameField(fModel.cells);
    }

    public void SpawnFixedLevel(int level_idx)
    {
        fModel.LoadLevelDataFixed(level_idx);
        fView.SpawnGameField(fModel.cells);
    }
}
