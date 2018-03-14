using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMeshMod : MonoBehaviour {
	public Transform trans;
	public string name;
	public TextMesh t;
	public CameraIdentity camID;
	public float fac=1.0f;
	// Use this for initialization
	void Start () {
		t=this.GetComponent<TextMesh>();

		name=camID.objName;
		t.transform.localScale*=fac;
	}

	// Update is called once per frame
	void Update () {
		if(trans==null)
		{
			trans=camID.attachedTrans;
		}
		t.transform.rotation=Quaternion.identity;
		t.transform.Rotate(new Vector3(90,0,0));
		t.text="";
		//t.text+=name+trans.position.ToString();
		t.text+=name;
	}
}
