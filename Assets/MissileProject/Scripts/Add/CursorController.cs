using UnityEngine;
using System.Collections;

public class CursorController : MonoBehaviour {
	public Transform tran;
	public Camera cam;
	public CameraIdentity camID;
	public Vector3 size;
	public float fac=1.0f;
	// Use this for initialization
	void Start () {
		cam=GameObject.Find("Camera_Top_Global").GetComponent<Camera>();
		tran=this.transform.parent.transform;
		transform.localScale=Vector3.one;
		size=Vector3.one/transform.lossyScale.x*0.1f*fac;
		camID=cam.gameObject.GetComponent<CameraIdentity>();
	}
	
	// Update is called once per frame
	void Update () {
		
		this.transform.position=tran.position;
		transform.localScale=size*cam.orthographicSize*0.1f*camID.scaleFactor;
	}
}
