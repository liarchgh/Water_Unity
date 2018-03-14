using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resizer : MonoBehaviour {
	public CameraController cam;
	public NavObjIdentity ID;
	public Vector3 size;
	// Use this for initialization
	void Start () {
		cam=GameObject.Find("Camera_Top_Global").GetComponent<CameraController>();
		size=transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale=size*cam.cam.orthographicSize*0.01f*ID.scaleFactor*cam.scaleFactor;
	}
}
