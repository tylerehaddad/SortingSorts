using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMovement : MonoBehaviour
{
	public float speed = 1f;
	public float movementMax = 5f;

	void Update()
	{
		transform.localPosition = new Vector3(movementMax * Mathf.Sin (Time.time * speed), 0f, 0f);
	}
}