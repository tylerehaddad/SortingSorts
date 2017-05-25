using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class DrawerBehavior : MonoBehaviour {

	public bool activeAtAwake = true;

	//[HideInInspector]
	public bool closed;
	[HideInInspector]
	public bool moving = false;

	public float speed = 0.125f;

	private RectTransform rectTransform;

	void Awake ()
	{
		gameObject.SetActive (activeAtAwake);
	}

	void Start()
	{
		rectTransform = gameObject.GetComponent<RectTransform> ();
		moving = false;
	}

	void Update ()
	{
		if (moving)
		{
			if (closed)
			{
				rectTransform.anchoredPosition = new Vector2 (0, Mathf.Lerp (rectTransform.anchoredPosition.y, 0, speed));

				if (rectTransform.anchoredPosition.y < 0.1)
				{
					closed = false;
					moving = false;
				}
			} else
			{
				rectTransform.anchoredPosition = new Vector2 (0, Mathf.Lerp (rectTransform.anchoredPosition.y, Screen.height * 3, speed));

				if (rectTransform.anchoredPosition.y > 0.9 * (Screen.height * 3))
				{
					closed = true;
					moving = false;
				}
			}
		} else
		{
			if (closed)
			{
				rectTransform.anchoredPosition = new Vector2 (0, Screen.height * 3);
			} else
			{
				rectTransform.anchoredPosition = new Vector2 (0, 0);
			}
		}
	}

	public void Move()
	{
		if (moving)
		{
			if (closed)
			{
				closed = false;
			} else
			{
				closed = true;
			}
		} else
		{
			moving = true;
		}
	}

	public void Close()
	{
		if (closed)
		{
			closed = false;
		}

		moving = true;
	}

	public void Open()
	{
		if (!closed)
		{
			closed = true;
		}

		moving = true;
	}
}
