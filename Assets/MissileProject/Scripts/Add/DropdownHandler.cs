using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Suimono.Core;
public class DropdownHandler : MonoBehaviour
{
	public CameraIdentity defaultCamera;
	//public RenderTexture clearTex;
	public bool isMain=false;
	public TagMod tag;
	//public Transform trans;
	public Dropdown d;
	public RawImage screen;
	ScreenMan sm;
	public int id,prev=0;
	void Start ()
	{
		d = this.GetComponent<Dropdown> ();
		sm = GameObject.Find ("UI_Canvas").GetComponent<ScreenMan> ();
//		if(tag!=null)
//			tag.gameObject.SetActive(false);
	}
	public void ChangeScreenTo(CameraIdentity camID,RenderTexture r)
	{
		screen.gameObject.SetActive(true);
		if(tag!=null)
			tag.gameObject.SetActive(false);
		if(screen!=null)
		{
			screen.texture=r;
		}
		camID.isMain=isMain;
	}
	public void ChangeScreenTo(CameraIdentity camID,RenderTexture r,Transform t,string objName)
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
			screen.texture=r;
		}
		camID.isMain=isMain;
	}
	public void ChangeScreenTo()
	{
		if(tag!=null)
			tag.gameObject.SetActive(false);
		if(screen!=null)
		{
			screen.gameObject.SetActive(false);
		}
	}
	public void StatusChange ()
	{
		sm.ChangeScreen(this,d.value);
		prev=d.value;
	}
}


//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;
//using Suimono.Core;
//public class DropdownHandler : MonoBehaviour
//{
//	public TagMod tag;
//	//public Transform trans;
//	public Dropdown d;
//	ScreenMan sm;
//	public Camera prev;
//	public int id;
//	public float depth;
//	public bool hasInit;
//	void Start ()
//	{
//		prev = new Camera ();
//		d = this.GetComponent<Dropdown> ();
//		sm = GameObject.Find ("UI_Canvas").GetComponent<ScreenMan> ();
//		hasInit = false;
//	}
//	void Update()
//	{
//		if (!hasInit&&id==-1) {
//			if (prev == null) {
//				prev = sm.cam_main;
//				hasInit = true;
//			}
//		}
//	}
//	public void StatusChange ()
//	{
//		if(id!=-1)
//			tag.gameObject.SetActive(false);
//		Rect t = new Rect (0, 0, 1, 1);
//		if (id >= 0) {
//			t = new Rect (0.8f, 0 + 0.2f * id, 0.2f, 0.2f);
//		} else if (id == -1) {
//			t = new Rect (0, 0, 1, 1);
//
//		}
//		if (prev != null) {
//			prev.depth = depth;
//			prev.gameObject.SetActive (false);
//		}
//		if (d.value == 0) {
//		} else if (d.value == 1) {
//			sm.cam_up_global.gameObject.SetActive (true);
//			sm.cam_up_global.rect = t;
//			sm.cam_up_global2.rect = t;
//			prev = sm.cam_up_global;
//			if (id == -1) {
//				sm.cam_up_global_mov.enabled = true;
//				sm.cam_up_global_scale.enabled = true;
//				sm.cam_up_global2_scale.enabled = true;
//			} else {
//				sm.cam_up_global_mov.enabled = false;
//				sm.cam_up_global_scale.enabled = false;
//				sm.cam_up_global2_scale.enabled = false;
//			}
//		} else if (d.value == 2) {
//			sm.cam_main.gameObject.SetActive (true);
//			sm.cam_main.gameObject.GetComponent<Suimono_UnderwaterFog> ().re = t;
//			prev = sm.cam_main;
//			if (id == -1) {
//				sm.cam_main_mov.enabled = true;
//			} else {
//				sm.cam_main_mov.enabled = false;
//			}
//		} else {
//			//Debug.Log(d.value);
//			sm.cams [d.value - 3].gameObject.SetActive (true);
//			sm.cams [d.value - 3].rect = t;
//			prev = sm.cams [d.value - 3];
//			tag.gameObject.SetActive(true);
//			tag.trans=prev.GetComponent<CameraIdentity>().trans;
//			//trans=tag.trans;
//			//tag.trans=trans;
//		}
//		if (id == -1) {
//			depth = prev.depth;
//			prev.depth = -9999;
//		}
//	}
//}
