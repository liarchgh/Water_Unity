using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpperTagMod : MonoBehaviour {
	public Text tag;
	public Transform FPSCam;
	public float offsetHeight=0;
	public NavObjIdentity ID;
	public ObjGen og;
	public int tag_id = 0;
	public Camera focus=null;
	// Use this for initialization
	void Start () {
		FPSCam = GameObject.Find ("FirstPersonCharacter").transform;
		tag.rectTransform.position = (new Vector3 (0, (30 + offsetHeight)*this.transform.localScale.y, 0));
		og = GameObject.Find ("ObjGenerator").GetComponent<ObjGen> ();
	}

	// Update is called once per frame
	void Update () {
		if (og.drop_Datas [og.drops [tag_id].prev].cam != null) {
			focus = og.drop_Datas [og.drops [tag_id].prev].cam;
			this.transform.LookAt (focus.transform, focus.transform.up);
		}
	}
	public void ChangeText(string s)
	{
		tag.text = s;
	}
}
