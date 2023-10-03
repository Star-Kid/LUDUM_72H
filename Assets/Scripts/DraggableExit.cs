using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableExit : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject pObjectForDrag;
    GameObject objectForDrag;

    public void OnBeginDrag(PointerEventData eventData)
    {
        objectForDrag = Instantiate(pObjectForDrag);
        objectForDrag.transform.localRotation = Quaternion.Euler(90, 0, 0);
        UpdateDragObjectPosition();
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateDragObjectPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(objectForDrag);

        float distance = 50f;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance))
        {
            Debug.Log("Hit! (by exit) " + hit.transform.gameObject.name);
            hit.transform.parent.GetComponent<FieldView>().GetDragEndHitCoordsForExit (hit.transform.gameObject);
        }
    }

    void UpdateDragObjectPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 1.8f;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        objectForDrag.transform.position = worldPosition;
    }
}
