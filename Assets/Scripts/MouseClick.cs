using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClick : MonoBehaviour
{
    public GameObject Thiss;
    public GameObject Prefab;
    Vector3 mouse_pos;
    public void Update()
    {
        mouse_pos = Input.mousePosition;
    }
    public void Down()
    {
        Destroy(Thiss);
        Instantiate(Prefab, new Vector3 (mouse_pos.x, mouse_pos.y, 0), Quaternion.identity);
    }
}
