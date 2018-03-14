using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexMod2 : MonoBehaviour {
	public Transform trans;
	public Vector3 localTrans;
	public string name;
	public TextMesh t;
	public CameraIdentity camID,mapID;
	public Camera cam;
	public Vector3 size;
	// Use this for initialization
	void Start () {
		t=this.GetComponent<TextMesh>();
		localTrans=transform.position;
		name=camID.objName;
		cam=GameObject.Find("Camera_Top_Global").GetComponent<Camera>();
		mapID=cam.gameObject.GetComponent<CameraIdentity>();
		size=transform.localScale;

	}

	// Update is called once per frame
	void Update () {
		if(trans==null)
		{
			trans=camID.attachedTrans;
		}
		t.transform.position=trans.position+localTrans;
		t.transform.rotation=Quaternion.identity;
		t.transform.Rotate(new Vector3(90,0,0));
		t.text="";
		//t.text+=name+trans.position.ToString();
		t.text+=name;
		transform.localScale=size*cam.orthographicSize*0.01f*mapID.scaleFactor;
	}
}
