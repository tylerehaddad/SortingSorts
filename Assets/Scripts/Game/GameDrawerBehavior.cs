using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDrawerBehavior : MonoBehaviour 
{
	//[HideInInspector]
	public bool closed;

	public float closedPosition;
	public float openedPosition;

	private Vector3 startPosition;
	[HideInInspector]
	public bool moving = false;

	public float speed = 0.03125f;

	float percentage = 0;

	private AudioSource source;

	public AudioClip closeClip;
	public AudioClip stopClip;

	void Start()
	{
		source = GetComponent<AudioSource>();
		moving = false;
		startPosition = transform.localPosition;

		if (closed)
		{
			percentage = 1;
		}
	}

	void Update ()
	{
		transform.localPosition = new Vector3 (startPosition.x, startPosition.x + Mathf.Lerp (openedPosition, closedPosition, percentage), startPosition.z);

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
