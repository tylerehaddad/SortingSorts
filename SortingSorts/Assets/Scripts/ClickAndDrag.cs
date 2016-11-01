using UnityEngine;
using System.Collections;

public class ClickAndDrag : MonoBehaviour
{
    public GameObject selectedBlock;
	public Phrases phrasesRef;
    public Vector3 GOCenter;
    public Vector3 touchPosition;
    public Vector3 offset;
    public Vector3 newGOCenter;

	public LayerMask blocks, underscores;

    RaycastHit hit;

    public bool dragging = false;
	private float clickTimer = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit,100, blocks))
            {
                selectedBlock = hit.collider.gameObject;
                GOCenter = selectedBlock.transform.position;
                touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                offset = touchPosition - GOCenter;
                dragging = true;
				clickTimer = 0;
				if (Physics.Raycast (ray, out hit, 100, underscores)) {
					hit.collider.gameObject.GetComponent<Underscore> ().taken = false;
					selectedBlock.transform.SetParent (null);
				}

            }
        }

        if (Input.GetMouseButton(0))
        {
            if (dragging)
            {
				clickTimer += Time.deltaTime;
                touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                newGOCenter = touchPosition - offset;
                selectedBlock.transform.position = new Vector3(newGOCenter.x, newGOCenter.y, GOCenter.z);

            }
        }



        if (Input.GetMouseButtonUp(0))
        {
			if (dragging) 
			{
				if (clickTimer < 0.2f) 
				{
					Underscore u = phrasesRef.getNextUnderscore ();
					if (u != null) 
					{
						u.taken = true;
						selectedBlock.transform.position = new Vector2(u.transform.position.x, u.transform.position.y + 0.5f);
						selectedBlock.transform.SetParent (u.transform);
					}

				}
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (ray, out hit, 100, underscores)) 
				{
					Underscore u = hit.collider.gameObject.GetComponent<Underscore> ();
					if (!u.taken) 
					{
						u.taken = true;
						selectedBlock.transform.position = new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y + 0.5f);
						selectedBlock.transform.SetParent (hit.collider.transform);
					}
				}

				dragging = false;
			}
        }
    }
}
