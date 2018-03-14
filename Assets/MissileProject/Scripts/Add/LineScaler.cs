using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScaler : MonoBehaviour {
    //public TrailRenderer trail;
    public LineRenderer line;
	public Camera cam;
	public CameraIdentity camID;
	public float size;
	// Use this for initialization
	void Start () {
		cam=GameObject.Find("Camera_Top_Global").GetComponent<Camera>();
		//trail=this.GetComponent<TrailRenderer>();
		//size=trail.widthMultiplier;
        line = this.GetComponent<LineRenderer>();
        size = line.widthMultiplier;
        camID =cam.gameObject.GetComponent<CameraIdentity>();
	}

	// Update is called once per frame
	void Update () {
		//trail.widthMultiplier=size*cam.orthographicSize*0.5f*camID.scaleFactor;
        line.widthMultiplier = size * cam.orthographicSize * 0.5f * camID.scaleFactor;
    }
}
