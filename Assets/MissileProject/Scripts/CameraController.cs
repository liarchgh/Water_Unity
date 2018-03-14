﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public Camera cam,subcam;
	public Vector3 start_point;
	public float start_scale;
	public Quaternion start_rotation;
	public float start_FOV;
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 5F;
	public float sensitivityY = 5F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;
	public GameObject StartPot,rotationTarget,relativeSP;
	float rotationY = 0F;
	float speed=20f;
	public float scaleFactor=1f;

	public bool isTrackable=false,isMapCam=false,isInteractable=false,isAttachedWithPos=false,isMain=false,isTrackedObj=false,isRotationEnabled=true;
	// Use this for initialization
	void Start () {
		cam=this.GetComponent<Camera>();
		start_point = transform.position;
		start_rotation=transform.rotation;
		start_scale=cam.orthographicSize;
		start_FOV=cam.fieldOfView;
		CamSwitch (false);
	}
	// Update is called once per frame
	void Update () {
		if(isMapCam)
		{
			if(isMain)
			{
				scaleFactor=1f;
			}else{
				scaleFactor=5f;
			}
		}
		if(isMapCam&&!isMain)
		{
			if (Input.GetKey(KeyCode.UpArrow)||Input.GetKey(KeyCode.Keypad8))
			{
				if(cam.orthographic)
				{
					transform.Translate(Vector3.forward * Time.deltaTime *cam.orthographicSize,Space.World);
				}else{
					transform.Translate(Vector3.forward * Time.deltaTime *speed);
				}
			}
			if ( Input.GetKey(KeyCode.DownArrow)||Input.GetKey(KeyCode.Keypad2))
			{
				if(cam.orthographic)
				{
					transform.Translate(Vector3.back * Time.deltaTime *cam.orthographicSize,Space.World);
				}else{
					transform.Translate(Vector3.back * Time.deltaTime * speed);
				}
			}
			if (Input.GetKey(KeyCode.LeftArrow)||Input.GetKey(KeyCode.Keypad4))
			{
				if(cam.orthographic)
				{
					transform.Translate(Vector3.left * Time.deltaTime * cam.orthographicSize,Space.World);
				}else{
					transform.Translate(Vector3.left * Time.deltaTime * speed);
				}
			}
			if (Input.GetKey(KeyCode.RightArrow)||Input.GetKey(KeyCode.Keypad6))
			{
				if(cam.orthographic)
				{
					transform.Translate(Vector3.right * Time.deltaTime *cam.orthographicSize,Space.World);
				}else{
					transform.Translate(Vector3.right * Time.deltaTime *speed);
				}
			}
			if (Input.GetKey(KeyCode.Slash))
			{
				ResetCam();

			}

			if (Input.GetKey(KeyCode.Minus)||Input.GetKey(KeyCode.Keypad7))
			{
				if(cam.fieldOfView<=100)
					cam.fieldOfView +=2;
				if(cam.orthographic)
				{
					if(subcam!=null)
						subcam.orthographicSize+=10F;
					cam.orthographicSize+=10F;
				}
			}
			//Zoom in
			if (Input.GetKey(KeyCode.Equals)||Input.GetKey(KeyCode.Keypad9))
			{
				if(cam.fieldOfView>2)
					cam.fieldOfView-=2;
				if(cam.orthographic&&cam.orthographicSize>10)
				{
					if(subcam!=null)
						subcam.orthographicSize-=10F;
					cam.orthographicSize-=10F;
				}
			}
			//Zoom out
		}
		if(cam.isActiveAndEnabled&&isMain&&isInteractable)
		{
			if(Input.GetMouseButton(1)&&isRotationEnabled)
			{
				if (axes == RotationAxes.MouseXAndY)
				{
					float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

					rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
					rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

					transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
				}
				else if (axes == RotationAxes.MouseX)
				{
					transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
				}
				else
				{
					rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
					rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

					transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
				}
			}

			if (Input.GetKey(KeyCode.W))
			{
				if(cam.orthographic)
				{
					transform.Translate(Vector3.forward * Time.deltaTime *cam.orthographicSize,Space.World);
				}else{
					transform.Translate(Vector3.forward * Time.deltaTime *speed);
				}
			}
			if (Input.GetKey(KeyCode.S))
			{
				if(cam.orthographic)
				{
					transform.Translate(Vector3.back * Time.deltaTime *cam.orthographicSize,Space.World);
				}else{
					transform.Translate(Vector3.back * Time.deltaTime * speed);
				}
			}
			if (Input.GetKey(KeyCode.A))
			{
				if(cam.orthographic)
				{
					transform.Translate(Vector3.left * Time.deltaTime * cam.orthographicSize,Space.World);
				}else{
					transform.Translate(Vector3.left * Time.deltaTime * speed);
				}
			}
			if (Input.GetKey(KeyCode.D))
			{
				if(cam.orthographic)
				{
					transform.Translate(Vector3.right * Time.deltaTime *cam.orthographicSize,Space.World);
				}else{
					transform.Translate(Vector3.right * Time.deltaTime *speed);
				}
			}
			if (Input.GetKey(KeyCode.Space))
			{
				transform.Translate(Vector3.up * Time.deltaTime * speed,Space.World);
			}
			if (Input.GetKey(KeyCode.LeftControl))
			{
				transform.Translate(Vector3.down * Time.deltaTime *speed,Space.World);

			}
			if (Input.GetKey(KeyCode.Escape))
			{
				ResetCam();

			}

			if (Input.GetAxis("Mouse ScrollWheel") < 0)
			{
				if(cam.fieldOfView<=100)
					cam.fieldOfView +=2;
				if(cam.orthographic)
				{
					if(subcam!=null)
						subcam.orthographicSize+=10F;
					cam.orthographicSize+=10F;
				}
			}
			//Zoom in
			if (Input.GetAxis("Mouse ScrollWheel") > 0)
			{
				if(cam.fieldOfView>2)
					cam.fieldOfView-=2;
				if(cam.orthographic&&cam.orthographicSize>10)
				{
					if(subcam!=null)
						subcam.orthographicSize-=10F;
					cam.orthographicSize-=10F;
				}
			}
			//Zoom out
			if(rotationTarget!=null)
			{
				if(Input.GetKey(KeyCode.Q))
				{
					transform.RotateAround(rotationTarget.transform.position,Vector3.up,speed*Time.deltaTime);
				}
				if(Input.GetKey(KeyCode.E))
				{
					transform.RotateAround(rotationTarget.transform.position,Vector3.up,-speed*Time.deltaTime);
				}
			}
		}
	}
	public void ResetCam()
	{
		//Debug.Log ("Reset" + StartPot.transform.position);
		if (StartPot != null)
		{
			transform.position = StartPot.transform.position;
			transform.rotation = StartPot.transform.rotation;
		}else{
			transform.position = start_point;
			transform.rotation = start_rotation;
		}
		cam.fieldOfView=start_FOV;
		cam.orthographicSize=start_scale;
		if(subcam!=null)
		{
			subcam.transform.position = start_point;
			subcam.transform.rotation = start_rotation;
			subcam.orthographicSize=start_scale;
		}
	}
	public void CamSwitch(bool b)
	{
		cam.enabled = b;
		if (subcam != null) {
			subcam.enabled = b;
		}
	}
}