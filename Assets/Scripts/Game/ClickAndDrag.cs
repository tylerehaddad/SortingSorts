﻿using UnityEngine;
using System.Collections;

public class ClickAndDrag : MonoBehaviour
{
    public GameObject selectedBlock;
	public Transform drawer;
    public Vector3 GOCenter;
    public Vector3 touchPosition;
    public Vector3 offset;
    public Vector3 newGOCenter;
	public GameManager gameManager;
	private AudioSource source;

	public AudioClip dropClip;

	public LayerMask blocks, underscores;

    RaycastHit hit;

    public bool dragging = false;
	private float clickTimer = 0;

	void Start()
	{
		source = GetComponent<AudioSource>();
	}

    void Update()
    {
		if (gameManager.paused == false)
		{
			if (Input.GetMouseButtonDown (0))
			{
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast (ray, out hit, 100, blocks))
				{
					selectedBlock = hit.collider.gameObject;
					GOCenter = selectedBlock.transform.position;
					touchPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
					offset = touchPosition - GOCenter;
					dragging = true;

					selectedBlock.GetComponent<SpriteRenderer> ().sortingOrder = 19;
					selectedBlock.GetComponentsInChildren<SpriteRenderer> () [1].sortingOrder = 18;

					if (gameManager.grabTutorial == true)
					{
						gameManager.grabTutorial = false;
						gameManager.placeTutorial = true;
					}

					clickTimer = 0;
					if (Physics.Raycast (ray, out hit, 100, underscores))
					{
						hit.collider.gameObject.GetComponent<Underscore> ().taken = false;
						selectedBlock.transform.SetParent (null);
						selectedBlock.transform.position = new Vector2 (selectedBlock.GetComponent<Letter> ().startPos.x, selectedBlock.GetComponent<Letter> ().startPos.y + drawer.position.y);
						dragging = false;
					}

				}
			}

			if (Input.GetMouseButton (0))
			{
				if (dragging)
				{
					clickTimer += Time.deltaTime;
					touchPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
					newGOCenter = touchPosition - offset;
					selectedBlock.transform.position = new Vector3 (newGOCenter.x, newGOCenter.y, GOCenter.z);
				}
			}

			if (Input.GetMouseButtonUp (0))
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
							selectedBlock.transform.position = new Vector2 (hit.collider.transform.position.x, hit.collider.transform.position.y + 0.5f);
							selectedBlock.transform.SetParent (hit.collider.transform);

							selectedBlock.GetComponent<SpriteRenderer> ().sortingOrder = 17;
							selectedBlock.GetComponentsInChildren<SpriteRenderer> () [1].sortingOrder = 16;
						}
					} else
					{
						selectedBlock.transform.position = new Vector2 (selectedBlock.GetComponent<Letter> ().startPos.x, selectedBlock.GetComponent<Letter> ().startPos.y + drawer.position.y);
						selectedBlock.GetComponent<SpriteRenderer> ().sortingOrder = 1;
						selectedBlock.GetComponentsInChildren<SpriteRenderer> () [1].sortingOrder = -1;
					}

					source.clip = dropClip;
					source.Play ();

					dragging = false;
				}
			}
		}
    }
}
