using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Move Camera
/// </summary>
public class CameraControl : MonoBehaviour {
	public float speedH = 2.0f;
	public float speedV = 2.0f;

	private float yaw = 0.0f;
	private float pitch = 0.0f;
	private static CameraControl instance;
	public static Vector3 Rotation{
		get{
			return new Vector3(instance.pitch, instance.yaw, 0.0f);
		}
	}

	private void Awake() {
		if(CameraControl.instance==null)
			CameraControl.instance=this;
	}
	

	
	void Update () {
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}
