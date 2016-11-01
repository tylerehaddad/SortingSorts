using UnityEngine;
using System.Collections;

public class ClickAndDrag : MonoBehaviour
{
    public GameObject selectedBlock;

    public Vector3 GOCenter;
    public Vector3 touchPosition;
    public Vector3 offset;
    public Vector3 newGOCenter;

    RaycastHit hit;

    public bool dragging = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                selectedBlock = hit.collider.gameObject;
                GOCenter = selectedBlock.transform.position;
                touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                offset = touchPosition - GOCenter;
                dragging = true;
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (dragging)
            {
                touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                newGOCenter = touchPosition - offset;
                selectedBlock.transform.position = new Vector3(newGOCenter.x, newGOCenter.y, GOCenter.z);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }
    }
}
