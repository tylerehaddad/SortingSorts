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
	public GameManager gameManager;

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

				if (gameManager.grabTutorial == true)
				{
					gameManager.grabTutorial = false;
					gameManager.placeTutorial = true;
				}

				clickTimer = 0;
				if (Physics.Raycast (ray, out hit, 100, underscores)) {
					hit.collider.gameObject.GetComponent<Underscore> ().taken = false;
					selectedBlock.transform.SetParent (null);
					selectedBlock.transform.position = selectedBlock.GetComponent<Letter> ().startPos;
					dragging = false;
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
