using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class NavObjIdentity : MonoBehaviour {
	public Transform pos;					//Position of the nav obj
	public string fileName,filePath;        //File name and path
	public string objName,camName;			//Names
	public GameObject model;				//Model needed to instantiate
	public Camera cam;						//Its subcam
	public RenderTexture renderTex;			//RenderTexture to use
	public LineRenderer line, lineMap;		//Lines
	public bool isTrackedObj=false,isRotationLocked=false;
	public Transform dummyWithPos,dummyWithPosRotH,dummyWithPosRotHV;
	public NavObjPosUpdater mover;
	public GameObject bubble,startPot;
	public MeshRenderer mapIcon;
	public UpperTagMod[] upperTags;
	public MapTagMod mapTag;
	public float scaleFactor=1.0f;
	public Material mat,iconMat;
	public void Init()
	{
		Transform t_model=Instantiate (model).transform;
		t_model.SetParent (dummyWithPosRotHV);
		pos = dummyWithPos;

		filePath = Database.filePath;

		mover.Init ();
		foreach (UpperTagMod upperTag in upperTags) {
			upperTag.ChangeText (objName);
			upperTag.tag.color = mat.color;
		}
		mapTag.ChangeText (objName);
		mapTag.t.color = mat.color;
		if (renderTex != null) {
			cam.targetTexture = renderTex;
		}
		cam.enabled = false;
		if (isTrackedObj) {
			TagMod textPos=GameObject.Find("Text_Pos").GetComponent<TagMod>();
			textPos.AddEntry(pos,"<color=#"+ColorUtility.ToHtmlStringRGBA(mat.color)+">"+objName+"</color>");
			line.gameObject.SetActive (true);
			lineMap.gameObject.SetActive (true);
			line.material = mat;
			lineMap.material = mat;
		} else {
			line.gameObject.SetActive(false);
			lineMap.gameObject.SetActive(false);
		}
		if (iconMat != null) {
			mapIcon.material = iconMat;
		} else {
			mapIcon.gameObject.SetActive (false);
		}

	}
		
}
