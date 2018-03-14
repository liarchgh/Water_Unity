using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class ParamSettings : MonoBehaviour {
	public InputField filePath,umiDepth,preset_Name,cam_x,cam_y,cam_z;
	public Text placeholder_path,fileAvability_text;
	public int[] initCount;
	public bool[,] initTracked;
	public ContentSetup contentPanel,oriPanel;
	public Vector3 ScrollPos;
	public Button finishBtn;
	public ModeSettings prev;
	public UGUIFolderSelector folderSelector;
	public UGUIFileSelector fileSelector;
	public UGUIFileSaver fileSaver;
	/// <summary>
	/// EventResponsors
	/// </summary>

	public void OnSelection()
	{
		folderSelector.gameObject.SetActive (true);
		folderSelector.GetFile(filePath.text,GotFile, ".t"); 
	}
	public void OnPrev()
	{
		if (contentPanel != null) {
			Destroy (contentPanel.gameObject);
		}
		prev.gameObject.SetActive (true);
		finishBtn.interactable = false;
		this.gameObject.SetActive (false);
	}
	public void OnEndEdit()
	{
		
		if(TryToLoadFile(filePath.text+"/position1.txt"))
		{
			fileAvability_text.text = "<color=#"+ColorUtility.ToHtmlStringRGBA(Color.green)+">"+"文件夹有效"+"</color>";
			Database.filePath = filePath.text;
			FlushInitTracked ();
			TryToLoadFiles ();
		}else{
			fileAvability_text.text = "<color=#"+ColorUtility.ToHtmlStringRGBA(Color.red)+">"+"文件夹无效"+"</color>";
			if (contentPanel != null) {
				Destroy (contentPanel.gameObject);
			}
			finishBtn.interactable = false;
		}
	}

	public void OnFinish()
	{
		Database.umiDepth = float.Parse (umiDepth.text);
		if(!cam_x.text.Equals("")&&!cam_y.text.Equals("")&&!cam_z.text.Equals(""))
		{
			Debug.Log("SET");
			Database.initCamPosSet(float.Parse(cam_x.text),float.Parse(cam_y.text),float.Parse(cam_z.text));
			Database.isSetInitCamPos=true;
		}else{
			Database.isSetInitCamPos=false;
		}
		Database.initCountSet (5, initCount);
		Database.initTrackedSet (5, 100, initTracked);
		if (contentPanel != null) {
			Destroy (contentPanel.gameObject);
		}
		SceneManager.LoadScene ("MainVer1");
	}

	public void OnLoadPreset()
	{
		fileSelector.gameObject.SetActive (true);
		fileSelector.GetFile(Application.streamingAssetsPath + "/Presets",GotPreset, ".ini"); 

	}
	public void OnSavePreset()
	{		
		fileSaver.gameObject.SetActive (true);
		fileSaver.GetFile (Application.streamingAssetsPath + "/Presets", GotPresetFolder, ".ini");

	}
	void GotPreset(UGUISelectorBase.Status status,string path)
	{
		Debug.Log("File Status : "+status+", Path : "+path);
		if (status == UGUISelectorBase.Status.Cancelled) {
		}else{
			if (TryToLoadFile (path)) {
				LoadFile (path);
				TryToLoadFiles ();
			}
		}
	}
	void GotPresetFolder(UGUISelectorBase.Status status,string path)
	{
		Debug.Log("File Status : "+status+", Path : "+path);
		if (status != UGUISelectorBase.Status.Cancelled) {
			WriteFile (path);
		}
	}
	void GotFile(UGUISelectorBase.Status status, string path){
		Debug.Log("File Status : "+status+", Path : "+path);
		if (status == UGUISelectorBase.Status.Cancelled) {
		}else{
			if (path != "") {
//			int i = 0;
//			for (i = path.Length - 1; (path [i] != '/') && (path [i] != '\\') && (i >= 0); i--) {
//			}
//
//			filePath.text = path.Substring (0, i);

				filePath.text = path;
				filePath.text = filePath.text.Replace ('\\', '/');
				if (TryToLoadFile (filePath.text + "/position1.txt")) {
					fileAvability_text.text = "<color=#" + ColorUtility.ToHtmlStringRGBA (Color.green) + ">" + "文件夹有效" + "</color>";
					Database.filePath = filePath.text;
					FlushInitTracked ();
					TryToLoadFiles ();
				} else {
					fileAvability_text.text = "<color=#" + ColorUtility.ToHtmlStringRGBA (Color.red) + ">" + "文件夹无效" + "</color>";
					finishBtn.interactable = false;
					if (contentPanel != null) {
						Destroy (contentPanel.gameObject);
					}
				}
			}
		}
	}


	public void Init()
	{
		preset_Name.text = "未加载配置";
		filePath.text = Application.streamingAssetsPath;
		Database.filePath = Application.streamingAssetsPath;
		FlushInitTracked ();
		TryToLoadFiles ();
		fileAvability_text.text = "<color=#" + ColorUtility.ToHtmlStringRGBA (Color.green) + ">" + "已载入默认文件夹" + "</color>";
	}
	void FlushInitTracked()
	{		
		Debug.Log ("FLUSHING");
		initTracked = new bool[5,100];
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 100; j++) {
				initTracked [i,j] = true;
			}
		}
	}
	void TryToLoadFiles()
	{
		initCount = new int[5];
		initCount [0] = 0;
		while (TryToLoadFile (Database.filePath + "/position" + (initCount [0]+1) + ".txt")) {
			initCount [0]++;
		}
		initCount [1] = 0;
		while (TryToLoadFile (Database.filePath + "/position_tag" + (initCount [1]+1) + ".txt")) {
			initCount [1]++;
		}		
		initCount [2] = 0;
		while (TryToLoadFile (Database.filePath + "/position_submarine" + (initCount [2]+1) + ".txt")) {
			initCount [2]++;
		}
		initCount [3] = 0;
		while (TryToLoadFile (Database.filePath + "/position_ship" + (initCount [3]+1) + ".txt")) {
			initCount [3]++;
		}
		initCount [4] = 0;
		while (TryToLoadFile (Database.filePath + "/position_point" + (initCount [4]+1) + ".txt")) {
			initCount [4]++;
		}

		if (contentPanel != null) {
			Destroy (contentPanel.gameObject);
		}
		contentPanel = Instantiate (oriPanel).GetComponent<ContentSetup> ();
		contentPanel.transform.SetParent (this.transform);
		contentPanel.GetComponent<RectTransform> ().localPosition = ScrollPos;
		contentPanel.SetupBoolTable (initCount, initTracked);
		fileAvability_text.text = "<color=#" + ColorUtility.ToHtmlStringRGBA (Color.green) + ">" + "文件夹有效" + "</color>";
		finishBtn.interactable = true;
	}

	bool TryToLoadFile(string name)
	{
		try
		{
			// Create an instance of StreamReader to read from a file.
			// The using statement also closes the StreamReader.
			using (StreamReader sr = new StreamReader(name)) 
			{
				String line;
				// Read and display lines from the file until the end of 
				// the file is reached.
				if ((line = sr.ReadLine()) != null) 
				{
					return true;
				}else{
					return false;
				}
			}
		}
		catch (Exception e) 
		{
			// Let the user know what went wrong.
			Console.WriteLine("The file could not be read:");

			Console.WriteLine(e.Message);
			return false;
		}
	}
	private void WriteFile(string name)
	{
		  StreamWriter sw;          
		  FileInfo t = new FileInfo(name);          
			sw = t.CreateText();
		　　
			ProduceFile (sw);
		  　　sw.Close();
		  　　sw.Dispose();
	}
	void ProduceFile(StreamWriter sw)
	{
		sw.WriteLine (umiDepth.text);
		sw.WriteLine (filePath.text);
		sw.WriteLine (cam_x.text+' '+cam_y.text+' '+cam_z.text);
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 100; j++) {
				if (initTracked [i, j]) {
					sw.Write ('1');
				} else {
					Debug.Log (i + ' ' + j);
					sw.Write ('0');
				}
				sw.Write (' ');
			}
			sw.WriteLine ();
		}
	}
	void LoadFile(string name)
	{
		try
		{
			// Create an instance of StreamReader to read from a file.
			// The using statement also closes the StreamReader.
			using (StreamReader sr = new StreamReader(name)) 
			{
				int tmp=0;
				for(tmp=name.Length-1;tmp>0&&name[tmp]!='\\'&&name[tmp]!='/';tmp--)
				{
				}
				preset_Name.text=name.Substring(tmp+1);


				int i=0;
				String line;
				// Read and display lines from the file until the end of 
				// the file is reached.
				while ((line = sr.ReadLine()) != null) 
				{
					//Debug.Log("Scan");
					ScanFile(i,line);
					i++;
					Debug.Log(line);
				}
			}

		}

		catch (Exception e) 
		{
			// Let the user know what went wrong.
			Console.WriteLine("The file could not be read:");
			Console.WriteLine(e.Message);
		}
	}
	void ScanFile(int li,string line)
	{
		if (li == 1) {
			filePath.text = line;
			Database.filePath = line;
		} else if (li == 0) {
			umiDepth.text = line;
		}else if(li==2){
			string[] temp=line.Split(new char[]{' '});
			cam_x.text=temp[0];
			cam_y.text=temp[1];
			cam_z.text=temp[2];
		} else {
			string[] temp = line.Split (new char[]{' '});
			for(int i=0;i<100;i++)
			{
				if (int.Parse (temp [i]) == 1) {
					initTracked [li - 3,i] = true;
				} else {
					initTracked [li - 3,i] = false;
					Debug.Log (li - 3 + " " + i);
				}
			}
		}
	}
//	void Update()
//	{
//		for (int i = 0; i < 5; i++) {
//			for (int j = 0; j < 100; j++) {
//				if(!initTracked[i,j])
//					Debug.Log (i + ' ' + j + ':' + initTracked [i, j].ToString());
//			}
//		}
//	}
}
