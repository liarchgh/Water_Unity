using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGlobalScaler : MonoBehaviour {
	//public TrailRenderer trail;
	public LineRenderer line;
	public CameraController cam;
	public NavObjIdentity ID;

	public float size;
	// Use this for initialization
	void Start () {
		cam=GameObject.Find("Camera_Top_Global").GetComponent<CameraController>();
		//trail=this.GetComponent<TrailRenderer>();
		//size=trail.widthMultiplier;
		line = this.GetComponent<LineRenderer>();
		size = line.widthMultiplier;
	}

	// Update is called once per frame
	void Update () {
		//trail.widthMultiplier=size*cam.orthographicSize*0.5f*camID.scaleFactor;
		line.widthMultiplier = size * cam.cam.orthographicSize * 0.5f * ID.scaleFactor*cam.scaleFactor;
	}
}
