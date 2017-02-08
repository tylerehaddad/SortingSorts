using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinRotate : MonoBehaviour
{
	public float speed = 2f;
	public float rotationMax = 45f;

	void Update()
	{
		transform.rotation = Quaternion.Euler (0f, 0f, rotationMax * Mathf.Sin (Time.time * speed));
	}
}