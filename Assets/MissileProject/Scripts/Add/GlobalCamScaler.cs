using UnityEngine;
using System.Collections;

public class GlobalCamScaler : MonoBehaviour {
	public Camera cam;
	public float start_scale;
	// Use this for initialization
	void Start () {
		cam = this.GetComponent<Camera> ();
		start_scale=cam.orthographicSize;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Mouse ScrollWheel") <0)
		{
			if(cam.orthographicSize<=100)
				cam.orthographicSize+=1;
		}
		//Zoom in
		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			if(cam.orthographicSize>1)
				cam.orthographicSize-=1;
		}
		if (Input.GetKey(KeyCode.Escape))
		{
			cam.orthographicSize=start_scale;
		}
	}
}
