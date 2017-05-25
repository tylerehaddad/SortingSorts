using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(AudioSource))]
public class DrawerBehavior : MonoBehaviour {

	public bool activeAtAwake = true;

	//[HideInInspector]
	public bool closed;
	[HideInInspector]
	public bool moving = false;

	public float speed = 0.03125f;

	float percentage = 0;

	private RectTransform rectTransform;
	private AudioSource source;

	public AudioClip closeClip;
	public AudioClip stopClip;

	void Awake ()
	{
		gameObject.SetActive (activeAtAwake);
	}

	void Start()
	{
		rectTransform = gameObject.GetComponent<RectTransform> ();
		source = GetComponent<AudioSource>();
		moving = false;
	}

	void Update ()
	{
		rectTransform.anchoredPosition = new Vector2 (0, Mathf.Lerp (0, Screen.height * 3, percentage));

		if (moving)
		{
			if (closed)
			{
				percentage = Mathf.Max (percentage - speed, 0);
				if (percentage == 0)
				{
					closed = false;
					moving = false;

					source.clip = stopClip;
					source.Play();
				}
			} else
			{
				percentage = Mathf.Min (percentage + speed, 1);
				if (percentage == 1)
				{
					closed = true;
					moving = false;

					source.clip = stopClip;
					source.Play();
				}
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

		source.clip = closeClip;
		source.Play();
	}

	public void Close()
	{
		if (closed)
		{
			closed = false;
		}

		source.clip = closeClip;
		source.Play();

		moving = true;
	}

	public void Open()
	{
		if (!closed)
		{
			closed = true;
		}

		source.clip = closeClip;
		source.Play();

		moving = true;
	}
}
