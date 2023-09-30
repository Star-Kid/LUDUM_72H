using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_of_object : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    public Transform Mine;

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }
    private void OnMouseUp()
    {
        Mine.position = new Vector3(((int)Mine.position.x / 70) * 70 + 40,((int)Mine.position.y / 70) * 70 + 20);
        Debug.Log(((int)Mine.position.x) / 70);
    }
}