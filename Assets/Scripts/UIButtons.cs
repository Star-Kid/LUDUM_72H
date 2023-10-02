using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIButtons : MonoBehaviour
{
    public FieldController fieldController;
    public GameObject editorControls;

    public TMP_InputField ifWidth;
    public TMP_InputField ifHeight;

    public void SpawnFixedLevel(int level_idx)
    {
        fieldController.SpawnFixedLevel(level_idx);
        editorControls.SetActive(false);
    }

    public void SpawnRandomLevel()
    {
        fieldController.SpawnRandomLevel();
        editorControls.SetActive(false);
    }

    public void SpawnEditorLevelInitial()
    {
        fieldController.SpawnEditorLevel(10, 10);

        editorControls.SetActive(true);
        ifWidth.text = "10";
        ifHeight.text = "10";

        //int.TryParse(input.text, out int result);
        //number = result;`
    }

    public void SpawnEditorLevelWithSizes()
    {
        int width, height;
        int.TryParse(ifWidth.text, out int result);
        width = result;
        if (width < 4) width = 4; else if (width > 11) width = 11;

        int.TryParse(ifHeight.text, out int result2);
        height = result2;
        if (height < 4) height = 4; else if (height > 11) height = 11;

        fieldController.SpawnEditorLevel(width, height);
    }
}
