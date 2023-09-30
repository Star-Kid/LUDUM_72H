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
}
