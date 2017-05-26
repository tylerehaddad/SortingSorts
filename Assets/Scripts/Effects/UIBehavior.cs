using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBehavior : MonoBehaviour {

	//[HideInInspector]
	public bool hidden;
	//[HideInInspector]
	public bool moving = false;

	public float speed = 0.03125f;

	float percentage = 0;

	private RectTransform rectTransform;
	private AudioSource source;

	public AudioClip appearClip;
	public AudioClip hideClip;

	public Vector2 hiddenPos;
	public Vector2 shownPos;

	void Start()
	{
		source = GetComponent<AudioSource>();
		rectTransform = gameObject.GetComponent<RectTransform> ();
		moving = false;

		if (hidden)
		{
			percentage = 1;
		}
	}

	void Update ()
	{
		rectTransform.anchoredPosition = new Vector2 (Mathf.Lerp (shownPos.x, hiddenPos.x, percentage), Mathf.Lerp (shownPos.y, hiddenPos.y, percentage));

		if (moving)
		{
			if (hidden)
			{
				percentage = Mathf.Max (percentage - speed, 0);
				if (percentage == 0)
				{
					hidden = false;
					moving = false;
				}
			} else
			{
				percentage = Mathf.Min (percentage + speed, 1);
				if (percentage == 1)
				{
					hidden = true;
					moving = false;
				}
			}
		}
	}

	public void Move()
	{
		if (moving)
		{
			if (hidden)
			{
				hidden = false;
			} else
			{
				hidden = true;
			}
		} else
		{
			moving = true;
		}
	}

	public void Hide()
	{
		if (hidden)
		{
			hidden = false;
		}

		if (hideClip != null)
		{
			source.clip = hideClip;
			source.Play ();
		}

		moving = true;
	}

	public void Show()
	{
		if (!hidden)
		{
			hidden = true;
		}

		if (appearClip != null)
		{
			source.clip = appearClip;
			source.Play ();
		}

		moving = true;
	}
}
