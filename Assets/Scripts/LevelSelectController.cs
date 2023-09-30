using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    public GameObject buttonStart;
    public GameObject levelSelectUIGroup;

    public void ButtonStartClick()
    {
        buttonStart.SetActive(false);
        levelSelectUIGroup.SetActive(true);
    }

    public void LoadLevel(int level_index)
    {
        //��������� level_index � �����-�� �������������� ������ ��� ��� ����-������
        //SceneManager.LoadScene("scene_name");
    }
}
