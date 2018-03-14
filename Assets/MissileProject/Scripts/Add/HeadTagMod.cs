using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HeadTagMod : MonoBehaviour {
	public Text tag;
	public Transform trans,FPSCam;
	public float offsetHeight;
	// Use this for initialization
	void Start () {
		FPSCam = GameObject.Find ("FirstPersonCharacter").transform;
		tag.rectTransform.position = (new Vector3 (0, (30 + offsetHeight)*this.transform.localScale.y, 0));
	}
	
	// Update is called once per frame
	void Update () {
		//this.transform.position = new Vector3(trans.position.x,trans.position.y+offsetHeight,trans.position.z);
		this.transform.position = trans.position;

		this.transform.LookAt (FPSCam,FPSCam.up);
	}
}
