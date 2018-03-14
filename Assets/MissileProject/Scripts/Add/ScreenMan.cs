using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class ScreenMan : MonoBehaviour {
	public TagMod textPos;
	public Dropdown mainDrop;
	public List<RenderTexture> tex;
	public Camera[] cams;
	public CameraIdentity[] camIDs;
	public List<int> trackableCamID;
	public Camera cam_up_global,cam_up_global2,cam_main;
	public MouseLook cam_main_mov;
	public GlobalCamController cam_up_global_mov;
	public GlobalCamScaler cam_up_global_scale,cam_up_global2_scale;
	public Dropdown[] drops;
	public Dropdown addon;
	public List<string> cam_name_list;
	public DropdownHandler[] camToID;
	public int tmpID;
	public Transform umi;
	public GameObject initPanel,initPanel2,missilePrefab,tagPrefab;
	public Text initX,initY,initDepth,initMissile,initTag,initPath;
	public Material[] mats;
	public RenderTexture[] renderTexs;
	// Use this for initialization
	void Start () {
		Time.timeScale=0;

	}
	void GetCamNameList()
	{
		cam_name_list=new List<string>();

		foreach(CameraIdentity c in camIDs)
		{
			cam_name_list.Add(c.cam_tag);
		}
	}
	// Update is called once per frame
	void Init(Dropdown d)
	{
		d.ClearOptions();
		Dropdown.OptionData ttt;
		List<Dropdown.OptionData> tt = new List<Dropdown.OptionData>();
		ttt = new Dropdown.OptionData();
		ttt.text="无";
		tt.Add(ttt);
		foreach(string s in cam_name_list)
		{
			ttt = new Dropdown.OptionData();
			ttt.text=s;
			tt.Add(ttt);
		}
		d.AddOptions(tt);
	}
	public void ChangeScreen(DropdownHandler drop,int value)
	{
		if(drop.prev!=0)
		{
			camToID[drop.prev-1]=null;
		}
		if(value==0)
		{
			drop.ChangeScreenTo();
		}else{
			if(camToID[value-1]!=null)
			{
				camToID[value-1].d.value=0;
			}
			camToID[value-1]=drop;
			if(camIDs[value-1].isAttachedWithPos)
			{
				drop.ChangeScreenTo(camIDs[value-1],camIDs[value-1].tex,camIDs[value-1].attachedTrans,camIDs[value-1].objName);
			}else{
				drop.ChangeScreenTo(camIDs[value-1],camIDs[value-1].tex);
			}
		}
	}
	//Handlers
	public void OnInitSet()
	{
		
		if(umi!=null)
		{
			umi.position=new Vector3(float.Parse(initX.text),-1*float.Parse(initDepth.text),float.Parse(initY.text));
		}
		if(cam_up_global!=null&&cam_up_global2!=null)
		{
			cam_up_global.gameObject.transform.position=new Vector3(float.Parse(initX.text),cam_up_global.gameObject.transform.position.y,float.Parse(initY.text));
			cam_up_global2.gameObject.transform.position=new Vector3(float.Parse(initX.text),cam_up_global2.gameObject.transform.position.y,float.Parse(initY.text));		
		}

		GameObject temp;
		CameraIdentity tmpIdentity;
		for(int i=1;i<=int.Parse(initMissile.text);i++)
		{
			temp=Instantiate(missilePrefab);
			tmpIdentity=temp.GetComponentInChildren<CameraIdentity>();
			tmpIdentity.filePath = initPath.text;
			tmpIdentity.fileName="position"+i+".txt";
			tmpIdentity.cam_tag="T"+i+"后视";
			tmpIdentity.objName="T"+i;
			if(i<=renderTexs.Length)
			{
				tmpIdentity.GetComponent<Camera>().targetTexture=renderTexs[i-1];
			}
			if(i<=mats.Length)
			{
				//tmpIdentity.trailMap.material=mats[i-1];
				//tmpIdentity.trail.material=mats[i-1];
                tmpIdentity.lineMap.material = mats[i - 1];
                tmpIdentity.line.material = mats[i - 1];
				tmpIdentity.currentMat = mats [i - 1];
            }

		}
		Mover tmpMover;
		for(int i=1;i<=int.Parse(initTag.text);i++)
		{
			temp=Instantiate(tagPrefab);
			tmpMover=temp.GetComponentInChildren<Mover>();
			tmpMover.FilePath = initPath.text;
			tmpMover.FileName="position_tag"+i+".txt";
			tmpIdentity=temp.GetComponentInChildren<CameraIdentity>();
			tmpIdentity.objName=i+"#";
		}

		if(initPanel!=null)
		{
			initPanel.SetActive(false);
		}
		if(initPanel2!=null)
		{
			initPanel2.SetActive(true);
		}
		SceneInit();
		if(addon!=null)
		{
			addon.ClearOptions();
			Dropdown.OptionData ttt;
			List<Dropdown.OptionData> tt = new List<Dropdown.OptionData>();
			for(int i=0;i<trackableCamID.Count;i++)
			{
				ttt = new Dropdown.OptionData();
				ttt.text=camIDs[trackableCamID[i]].objName;
				tt.Add(ttt);
			}
			addon.AddOptions(tt);
		}
	}
	void SceneInit()
	{
		camToID=new DropdownHandler[500];
		GameObject[] temp=new GameObject[500];
		temp=GameObject.FindGameObjectsWithTag("missile");
		cams=new Camera[temp.Length];
		camIDs=new CameraIdentity[temp.Length];
		trackableCamID=new List<int>();
		for(int i=0;i<temp.Length;i++)
		{
			cams[i]=temp[i].GetComponent<Camera>();
			camIDs[i]=cams[i].GetComponent<CameraIdentity>();
			if(camIDs[i].isTrackable)
			{
				trackableCamID.Add(i);
				camIDs[i].isTrackedObj=false;
				//textPos.AddEntry(camIDs[i].attachedTrans,camIDs[i].objName);
			}
		}
		GetCamNameList();
		temp=GameObject.FindGameObjectsWithTag("drops");
		drops=new Dropdown[temp.Length];
		for(int i=0;i<temp.Length;i++)
		{
			drops[i]=temp[i].GetComponent<Dropdown>();
			Init(drops[i]);
			DropdownHandler tmpDH=drops[i].GetComponent<DropdownHandler>();
			if(tmpDH.defaultCamera!=null)
			{
				tmpID=0;
				for(int j=0;j<camIDs.Length;j++)
				{
					if(camIDs[j]==tmpDH.defaultCamera)
					{
						tmpID=j;
						break;
					}
				}
				drops[i].value=tmpID+1;
			}else{
				ChangeScreen(tmpDH,0);
			}
		}
	}
	public void OnAddTrackObj()
	{
		camIDs[trackableCamID[addon.value]].Track();
	}
	public void OnConfirmAdd()
	{
		if(initPanel2!=null)
		{
			initPanel2.SetActive(false);
		}
		Time.timeScale=1;
	}
}








//
//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;
//using System.Collections.Generic;
//public class ScreenMan : MonoBehaviour {
//	public RenderTexture[] tex;
//	public Camera[] cams;
//	public Camera cam_up_global,cam_up_global2,cam_main;
//	public MouseLook cam_main_mov;
//	public GlobalCamController cam_up_global_mov;
//	public GlobalCamScaler cam_up_global_scale,cam_up_global2_scale;
//	public Dropdown[] drops;
//	public List<string> cam_name_list;
//	// Use this for initialization
//	void Start () {
//		cam_up_global=GameObject.Find("Camera_Top_Global").GetComponent<Camera>();
//		cam_up_global2=GameObject.Find("Camera_Top_Global2").GetComponent<Camera>();
//		cam_main=GameObject.Find("FirstPersonCharacter").GetComponent<Camera>();
//		cam_main_mov = GameObject.Find ("FPSController").GetComponent<MouseLook>();
//		cam_up_global_mov = GameObject.Find ("Camera_Top_Global").GetComponent<GlobalCamController>();
//		cam_up_global_scale=GameObject.Find("Camera_Top_Global").GetComponent<GlobalCamScaler>();
//		cam_up_global2_scale=GameObject.Find("Camera_Top_Global2").GetComponent<GlobalCamScaler>();
//		cam_up_global.gameObject.SetActive(false);
//		GameObject[] temp=new GameObject[500];
//		temp=GameObject.FindGameObjectsWithTag("missile");
//		cams=new Camera[temp.Length];
//		for(int i=0;i<temp.Length;i++)
//		{
//			cams[i]=temp[i].GetComponent<Camera>();
//			cams[i].gameObject.SetActive(false);
//		}
//		GetCamNameList();
//		temp=GameObject.FindGameObjectsWithTag("drops");
//		drops=new Dropdown[temp.Length];
//		for(int i=0;i<temp.Length;i++)
//		{
//			drops[i]=temp[i].GetComponent<Dropdown>();
//			Init(drops[i]);
//		}
//
//	}
//	void GetCamNameList()
//	{
//		cam_name_list=new List<string>();
//
//		foreach(Camera c in cams)
//		{
//			cam_name_list.Add(c.GetComponent<CameraIdentity>().cam_tag);
//		}
//	}
//	// Update is called once per frame
//	void Init(Dropdown d)
//	{
//		d.ClearOptions();
//		Dropdown.OptionData ttt;
//		List<Dropdown.OptionData> tt = new List<Dropdown.OptionData>();
//		ttt = new Dropdown.OptionData();
//		ttt.text="无";
//		tt.Add(ttt);
//		ttt = new Dropdown.OptionData();
//		ttt.text="全局俯瞰";
//		tt.Add(ttt);
//		ttt = new Dropdown.OptionData();
//		ttt.text="主界面";
//		tt.Add(ttt);
//		foreach(string s in cam_name_list)
//		{
//			ttt = new Dropdown.OptionData();
//			ttt.text=s;
//			tt.Add(ttt);
//		}
//		d.AddOptions(tt);
//	}
//}
