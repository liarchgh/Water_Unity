using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class UGUIFileSelector : UGUISelectorBase {

	public override void Init()
	{
		if(startPos.x<=-240f)			//偏移修正
			startPos.x += 283.5f;
		///Dir
		text_Path.text=path;
		tmp_path = path;
		/// <summary>
		/// Folders
		/// </summary>
		currentPos = startPos;
		prevBtn = null;
		if (folders.Length > 0) {
			foreach (FolderBtn f in folders) {
				if(f!=null)
				if(f.gameObject!=null)
					Destroy (f.gameObject);
			}
		}
		if (fileBtns.Length > 0) {
			foreach (FileBtn f in fileBtns) {
				if(f!=null)
				if(f.gameObject!=null)
					Destroy (f.gameObject);
			}
		}
		string[] subDirs=GetChildDirectories(path);
		folders = new FolderBtn[subDirs.Length+1];
		int count = 0;
		if (!isRoot) {
			folders [0] = Instantiate (folderPrefab).GetComponent<FolderBtn> ();
			folders [0].transform.SetParent (rectPanel.transform);
			folders [0].GetComponent<RectTransform> ().localPosition = new Vector3 (currentPos.x, currentPos.y, currentPos.z);
			folders [0].Init (-1, "...", this);
			currentPos.x += intervX;
			count++;
		}
		for(int i=0;i<subDirs.Length;i++)
		{

			folders [count] = Instantiate (folderPrefab).GetComponent<FolderBtn> ();
			folders [count].transform.SetParent (rectPanel.transform);
			folders [count].GetComponent<RectTransform> ().localPosition = new Vector3 (currentPos.x, currentPos.y, currentPos.z);
			folders [count].Init (i, subDirs[i],this);
			if ((- 3 + count) % 4 == 0) {
				currentPos.x = startPos.x;
				currentPos.y += intervY;
			} else {
				currentPos.x += intervX;
			}
			count++;
		}

		string[] files = GetFiles(path, extension : extension);
		fileBtns=new FileBtn[files.Length];
		for(int i=0;i<files.Length;i++)
		{

			fileBtns [i] = Instantiate (iniFilePrefab).GetComponent<FileBtn> ();
			fileBtns [i].transform.SetParent (rectPanel.transform);
			fileBtns [i].GetComponent<RectTransform> ().localPosition = new Vector3 (currentPos.x, currentPos.y, currentPos.z);
			fileBtns [i].Init (i, files[i],this);
			if ((- 3 + count) % 4 == 0) {
				currentPos.x = startPos.x;
				currentPos.y += intervY;
			} else {
				currentPos.x += intervX;
			}
			count++;
		}
		rectPanel.sizeDelta = new Vector2 (rectPanel.sizeDelta.x, startPos.y-currentPos.y+100);
	}
}
