using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public struct Drop_Data
{
	public DropsHandler prev_drop;
	public NavObjIdentity obj;
	public Camera cam;
	public CameraController camCon;
	public int originCullingMask;

}
public class ObjGen : MonoBehaviour
{
	//public TagMod textPos;
	//public Dropdown mainDrop;
	//public List<RenderTexture> tex;
	//public Camera[] cams;
	public CameraController cam_up_global, cam_FPS;
	public Camera cam_blank;
	public DropsHandler[] drops;
	public Drop_Data[] drop_Datas;
	//public int tmpID;
	public Transform umi;
	public float initDepth = 20;

	public GameObject[] originPrefabs;			/// <summary>
	/// 0:missile 1:tag 2:submarine 3:ship 4:point
	/// </summary>
	public int[] initCount={2,4,0,1,0};
	public bool[,] initTracked;
	public List<NavObjIdentity>[] objs;
	public string initPath = "";
	public Material[] mats;
	//public GameObject[] models;
	public RenderTexture[] renderTexs;

	public List<Dropdown.OptionData> tt;
	// Use this for initialization
	void Start ()
	{
		OnInitSet ();
	}
	//Handlers
	public void OnInitSet ()
	{
		//Get data from database
		initDepth=Database.umiDepth;
		initPath = Database.filePath;
		initCount = Database.initCount;
		initTracked = Database.initTracked;
		//fakeData ();  //TODO:to remove
		int countMat = 1,countTexs=2,texCount=2;  //mat0 is white, tex0 is blank ,tex1 is main while tex2 is global; TODO:recover global change countTexs and texCount from 2 to 3
		foreach (int i in initCount) {
			texCount += i;
		}
		renderTexs = new RenderTexture[texCount];
		if (umi != null) {
			umi.position = new Vector3 (0, -1 *initDepth, 0);
		}

		drop_Datas = new Drop_Data[texCount];
		Dropdown.OptionData ttt;
		tt = new List<Dropdown.OptionData> ();
		ttt = new Dropdown.OptionData ();
		ttt.text = "无";
		tt.Add (ttt);
		drop_Datas [0].cam = cam_blank;
		drop_Datas [0].originCullingMask = cam_blank.cullingMask;
		ttt = new Dropdown.OptionData ();
		ttt.text = "自由视角";
		tt.Add (ttt);
		drop_Datas [1].camCon = cam_FPS;
		drop_Datas [1].cam = cam_FPS.cam;
		drop_Datas [1].originCullingMask = cam_FPS.cam.cullingMask;
		/*
		ttt = new Dropdown.OptionData ();
		ttt.text = "全局俯瞰";
		tt.Add (ttt);
		//TODO:REcover this
		drop_Datas [2].camCon = cam_up_global;*/
		GameObject temp;
		NavObjIdentity tmpIdentity;
		objs=new List<NavObjIdentity>[5];
		for (int i = 0; i <= 4; i++) {
			objs [i] = new List<NavObjIdentity> ();
			for (int j = 0; j < initCount [i]; j++) {
				temp = Instantiate (originPrefabs [i]);
				tmpIdentity = temp.GetComponentInChildren<NavObjIdentity> ();
				tmpIdentity.filePath = initPath;
				if (i == 0) {
					tmpIdentity.fileName = "position" + (j+1) + ".txt";
					tmpIdentity.camName = "T" + (j+1) + "后视";
					tmpIdentity.objName = "T" + (j+1);
				} else if (i == 1) {
					tmpIdentity.fileName = "position_tag" + (j+1) + ".txt";
					tmpIdentity.camName = (j+1) +"#" +  "后视";
					tmpIdentity.objName = (j+1)+"#";
				} else if (i == 2) {
					tmpIdentity.fileName = "position_submarine" + (j+1) + ".txt";
					tmpIdentity.camName = "潜艇" + (j+1) + "后视";
					tmpIdentity.objName = "潜艇" + (j+1);
				} else if (i == 3) {
					tmpIdentity.fileName = "position_ship" + (j+1) + ".txt";
					tmpIdentity.camName = "实验船" + (j+1) + "后视";
					tmpIdentity.objName = "实验船" + (j+1);
				} else if (i == 4) {
					tmpIdentity.fileName = "position_point" + (j+1) + ".txt";
					tmpIdentity.camName = "应答器" + (j+1) + "后视";
					tmpIdentity.objName = "应答器" + (j+1);
				}
				if (initTracked [i,j]) {//add track camtexs
					ttt = new Dropdown.OptionData ();
					ttt.text = tmpIdentity.camName;
					tt.Add (ttt);

						
					tmpIdentity.isTrackedObj = true;

					if (countTexs < renderTexs.Length) {
						renderTexs [countTexs] = new RenderTexture (1920, 1080, 24);
						tmpIdentity.renderTex = renderTexs [countTexs];
						drop_Datas [countTexs].cam = tmpIdentity.cam;
						drop_Datas [countTexs].originCullingMask = drop_Datas [countTexs].cam.cullingMask;
						drop_Datas [countTexs].obj = tmpIdentity;
						countTexs++;
					} else {
						Debug.Log ("OverCount");
					}
					if (countMat < mats.Length) {
						tmpIdentity.mat = mats [countMat];
						countMat++;
					} else {
						tmpIdentity.mat = mats [mats.Length - 1];
					}
				} else {
					tmpIdentity.mat = mats [0];
				}
				//tmpIdentity.iconMat = iconMats [i];
				//tmpIdentity.model = models [i];
				tmpIdentity.Init ();
				objs[i].Add (tmpIdentity);
			}
		}
		for(int i=0;i<6;i++) {
			drops[i].d.ClearOptions ();
			drops[i].d.AddOptions (tt);
			drops [i].ChangeScreenTo ();
		}
		drops [0].d.value = 1;

	}


	public void ChangeScreen(DropsHandler drop,int value)
	{
		if(value==0)
		{
			if (drop.prev != 0) {
				if (drop_Datas [drop.prev].camCon != null) {
					drop_Datas [drop.prev].camCon.CamSwitch (false);
				} else {
					drop_Datas [drop.prev].cam.enabled = false;
				}
				drop_Datas [drop.prev].prev_drop = null;
			}
			drop.ChangeScreenTo();
		}else{
			
			if(drop_Datas[value].prev_drop!=null&&drop_Datas[value].prev_drop!=drop)
			{

				drop_Datas [value].prev_drop.d.value = 0;

			}
			drop_Datas [value].prev_drop = drop;
			if (drop_Datas [value].obj != null) {
				drop.ChangeScreenTo (drop_Datas [value].cam, drop_Datas [value].obj.pos, drop_Datas [value].obj.objName);
			} else if (drop_Datas [value].camCon != null) {
				drop.ChangeScreenTo (drop_Datas [value].camCon);
			} else {
				drop.ChangeScreenTo (drop_Datas [value].cam);
			}
		}
		drop_Datas[value].cam.cullingMask=drop_Datas[value].originCullingMask|(1<<(13+drop.drop_id));
	}

	void fakeData()
	{
		initTracked = new bool[5,100];
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 100; j++) {
				initTracked [i,j] = false;
			}
		}
		initTracked [0,0] = true;
		initTracked [0,1] = true;
	}
}
