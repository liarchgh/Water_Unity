using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class UGUISelectorBase : MonoBehaviour {
	public enum Status
	{
		Successful, //Used if we successfully got a file
		Cancelled, //Used if the 'Cancel' button is clicked
		Failed, //Used if the Close() method is called while the window is open
		Destroyed, //Used if the instance is destroyed while the window is open
	}
	public SelectFileFunction Callback;
	public delegate void SelectFileFunction(Status status, string path);
	public string path = "";
	public string file = "";
	public string extension = ".*";
	public bool isRoot=false;
	public Text text_Path;
	public string tmp_path;

	public GameObject folderPrefab,iniFilePrefab;
	public FolderBtn[] folders;
	public FileBtn[] fileBtns;
	public BtnBase prevBtn;

	public Vector3 startPos,currentPos;
	public RectTransform rectPanel;
	public float intervX=160,intervY=-40;
	public virtual void Init()
	{
	}
	public void Confirm()
	{
		Debug.Log ("Confirm" + text_Path.text);
		Debug.Log (Callback);
		if (Callback != null) {

			Callback (Status.Successful, text_Path.text);
		}
		this.gameObject.SetActive (false);
	}
	public void Cancel()
	{
		if(Callback != null) Callback(Status.Cancelled, "");
		this.gameObject.SetActive (false);
	}
	public void Select(BtnBase f)
	{
		if (prevBtn != null) {
			prevBtn.img.color = Color.white;
		}
		prevBtn = f;
		if(f.no>=0)
			text_Path.text = tmp_path +"\\" +f.txt.text;
	}
	public void Deselect()
	{
		Debug.Log ("DESEL");
		if (prevBtn != null) {
			prevBtn.img.color = Color.white;
		}
		prevBtn = null;
		text_Path.text = tmp_path;
	}
	public void Prev()
	{
		if(!isRoot)
		{
			string[] pd=GetParentDirectories(path, true);
			path=pd[pd.Length-1];
			if(pd.Length<=1)
			{
				isRoot=true;
			}else{
				isRoot=false;
			}

			Init();
		}
	}
	public void Next(int i)
	{
		if (isRoot) {
			isRoot = false;
		}
		path = text_Path.text;
		Init();
	}


	public void GetFile(string startingDirectory, SelectFileFunction callback = null, string ext = ".*")
	{
		Callback = callback;
		extension = ext;
		path = startingDirectory;
		Init();
	}

	public void GetFile(SelectFileFunction callback = null, string extension = ".*")
	{
		GetFile(Application.dataPath, callback, extension);
	}


	public string[] GetParentDirectories(string filePath, bool includePaths = false)
	{
		List<string> parents = new List<string>();
		FileInfo fileInfo;

		while(true){
			try{
				fileInfo = new FileInfo(filePath);
				if(!includePaths) parents.Add(fileInfo.Directory.Name);
				else parents.Add(fileInfo.Directory.FullName);

				filePath = fileInfo.Directory.FullName;
			}

			catch{ break; }
		}

		parents.Reverse();
		return parents.ToArray();		
	}

	public string[] GetChildDirectories(string directoryPath, bool includePaths = false)
	{
		DirectoryInfo directory;

		if(Directory.Exists(directoryPath)) directory = new DirectoryInfo(directoryPath);
		else
		{
			try{ directory = new FileInfo(directoryPath).Directory;	}
			catch{ return new string[0]; }
		}

		List<string> children = new List<string>();

		try{
			DirectoryInfo[] directories = directory.GetDirectories();

			foreach(DirectoryInfo childDir in directories){
				if(!includePaths) children.Add(childDir.Name);
				else children.Add(childDir.FullName);
			}			
		}

		catch{
			children = new List<string>();
		}

		return children.ToArray();
	}

	public string[] GetFiles(string directoryPath, bool includePaths = false, string extension = ".*")
	{
		DirectoryInfo directory;

		if(Directory.Exists(directoryPath)) directory = new DirectoryInfo(directoryPath);
		else
		{
			try{ directory = new FileInfo(directoryPath).Directory;	}
			catch{ return new string[0]; }
		}

		List<string> files = new List<string>();

		try{
			FileInfo[] fileInfos = directory.GetFiles("*"+extension);

			foreach(FileInfo fileInfo in fileInfos){
				if(!includePaths) files.Add(fileInfo.Name);	
				else files.Add(fileInfo.FullName);	
			}
		}

		catch{
			files = new List<string>();
		}

		return files.ToArray();
	}
}
