using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableFurniture : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public PanelFurnitureForDragController furnitureInfo;
    public GameObject pObjectsForDragContainer;
    GameObject objectForDrag;

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Begin drag!");
        int idx = (int)furnitureInfo.fType;

        GameObject pObjectForDrag = pObjectsForDragContainer.GetComponent<CellView>().furnitures[idx];

        objectForDrag = Instantiate(pObjectForDrag);
        objectForDrag.GetComponent<ColorSetter>().SetColor(furnitureInfo.fColor);
        objectForDrag.transform.localScale = new Vector3(objectForDrag.transform.localScale.x * 0.125f,
                                                         objectForDrag.transform.localScale.y * 0.5f,
                                                         objectForDrag.transform.localScale.z * 0.125f);
        objectForDrag.transform.localRotation = Quaternion.Euler(0, 0, 0);
        UpdateDragObjectPosition();
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging...");
        UpdateDragObjectPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("End drag!");
        Destroy(objectForDrag);

        float distance = 50f;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance))
        {
            //draw invisible ray cast/vector
            Debug.DrawLine(ray.origin, hit.point);
            //log hit area to the console
            //Debug.Log(hit.point);
            Debug.Log("Hit! " + hit.transform.gameObject.name);
            hit.transform.parent.GetComponent<FieldView>().GetDragEndHitCoors(hit.transform.gameObject, furnitureInfo.fType, furnitureInfo.fColor);
        }
    }

    void UpdateDragObjectPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        //mousePos.z = Camera.main.nearClipPlane;
        mousePos.z = 1.8f;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        objectForDrag.transform.position = worldPosition;
    }
}
