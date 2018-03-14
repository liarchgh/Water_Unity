using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DropsHandler : MonoBehaviour {
	public Camera defaultCamera,clearCamera;
	public bool isMain=false;
	public Dropdown d;
	public RawImage screen;
	public TagMod tag;
	public ObjGen og;
	public int prev=0;
	public int drop_id=0;
	// Use this for initialization
	void Start () {
		d = this.GetComponent<Dropdown> ();
		og = GameObject.Find ("ObjGenerator").GetComponent<ObjGen> ();
	}
	
	public void ChangeScreenTo(CameraController camID)
	{
		
		screen.gameObject.SetActive(true);
		if(tag!=null)
			tag.gameObject.SetActive(false);
		if(screen!=null)
		{
			camID.CamSwitch (true);
			screen.texture=camID.cam.targetTexture;
		}
		camID.isMain=isMain;
	}
	public void ChangeScreenTo(Camera cam)
	{
		screen.gameObject.SetActive(true);
		if(tag!=null)
			tag.gameObject.SetActive(false);
		if(screen!=null)
		{
			cam.enabled = true;
			screen.texture=cam.targetTexture;
		}
	}
	public void ChangeScreenTo(Camera cam,Transform t,string objName)
	{
		//Debug.Log(objName);
		if(tag!=null)
		{
			tag.gameObject.SetActive(true);
			tag.SetEntry(t,objName);
		}
		screen.gameObject.SetActive(true);
		if(tag!=null)
			tag.gameObject.SetActive(true);
		if(screen!=null)
		{
			cam.enabled = true;
			screen.texture=cam.targetTexture;
		}
	}
	public void ChangeScreenTo()
	{
		if(tag!=null)
			tag.gameObject.SetActive(false);
		if (isMain) {
			if(clearCamera!=null)
				screen.texture = clearCamera.targetTexture;
		} else {
			if (screen != null) {
				screen.gameObject.SetActive (false);
			}
		}
	}
	public void StatusChange ()
	{
		Debug.Log (d.value);
		og.ChangeScreen(this,d.value);
		prev = d.value;
	}
}
