using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

public struct Node_data
{
	public float time;
	public float x, y, z;
}

public class NavObjPosUpdater : MonoBehaviour
{
	
	public NavObjIdentity ID;
	public ArrayList arrlist;
	public List<Node_data> nodeList;
	public float dt = 0.1f, scaleFactor = 0.1f;
	public int ptr;
	public string fileName = "position.txt";
	public string filePath = "";
	public Replayer rep_timer;
	public CameraController cam;

	public void Init ()
	{
		if (ID != null) {
			fileName = ID.fileName;
			filePath = ID.filePath;
		}
		if (filePath.Equals ("")) {
			LoadFile (Application.streamingAssetsPath + '/' + fileName);
		} else {
			LoadFile (filePath + '/' + fileName);
		}
		ptr = 0;
		rep_timer = GameObject.Find ("MyTimer").GetComponent<Replayer> ();
		rep_timer.AddRepObj (this);

		if (fileName.Equals ("position1.txt")) {
			Debug.Log (ID.objName);
			cam = GameObject.Find ("FirstPersonCharacter").GetComponent<CameraController> ();
			if(!Database.isSetInitCamPos)
			{
				cam.StartPot = ID.startPot;
				cam.rotationTarget = ID.dummyWithPos.gameObject;
				StartCoroutine (CallCamReset ());
			}else{
				Debug.Log(Database.initCamPos);
				cam.start_point=new Vector3(Database.initCamPos.x,-Database.initCamPos.z,Database.initCamPos.y)*scaleFactor;
				cam.ResetCam();
			}
		}
		InvokeRepeating ("UpdatePos", 0, dt);
	}


	void UpdatePos ()
	{
		if (ptr == 0) {
			///Reset Lines
			if (ID != null) {
				if (ID.isTrackedObj) {
					ID.line.positionCount = 0;
					ID.lineMap.positionCount = 0;
				}
			}
		}

		if (ptr < nodeList.Count - 1) {
			while (nodeList [ptr].time * 0.25 < rep_timer.currentTime && ptr < nodeList.Count - 1) {//TODO:remove *0.25
				ID.pos.position = new Vector3 (nodeList [ptr].x, -nodeList [ptr].z, nodeList [ptr].y) * scaleFactor;
				if (ID != null) {
					if (ID.isTrackedObj) {
						ID.line.positionCount++;
						ID.line.SetPosition (ptr, new Vector3 (nodeList [ptr].x, -nodeList [ptr].z, nodeList [ptr].y) * scaleFactor);
						ID.lineMap.positionCount++;
						ID.lineMap.SetPosition (ptr, new Vector3 (nodeList [ptr].x, -nodeList [ptr].z, nodeList [ptr].y) * scaleFactor);
					}
				}
				if (ID.isRotationLocked) {
					if (new Vector3 (nodeList [ptr + 1].x - nodeList [ptr].x, -nodeList [ptr + 1].z + nodeList [ptr].z, nodeList [ptr + 1].y - nodeList [ptr].y).sqrMagnitude <= 50) {
					}
				}else{
						ID.dummyWithPosRotH.LookAt (new Vector3 (nodeList [ptr + 1].x, -nodeList [ptr].z, nodeList [ptr + 1].y) * scaleFactor);
						ID.dummyWithPosRotHV.LookAt (new Vector3 (nodeList [ptr + 1].x, -nodeList [ptr + 1].z, nodeList [ptr + 1].y) * scaleFactor);
				}
				ptr++;
			}


			//Debug.Log(transform.position);

			if (ptr >= nodeList.Count - 1) {
				StartCoroutine (RemoveBubble ());
			}
		}
	}

	void LoadFile (string name)
	{

		arrlist = new ArrayList ();
		nodeList = new List<Node_data> ();
		try {
			// Create an instance of StreamReader to read from a file.
			// The using statement also closes the StreamReader.
			using (StreamReader sr = new StreamReader (name)) {
				String line;
				// Read and display lines from the file until the end of 
				// the file is reached.
				while ((line = sr.ReadLine ()) != null) {
					//Debug.Log("Scan");
					arrlist.Add (line);
					ScanFile (line);
				}
			}
		} catch (Exception e) {
			// Let the user know what went wrong.
			Console.WriteLine ("The file could not be read:");
			Console.WriteLine (e.Message);
		}
	}

	void ScanFile (string line)
	{
		//Debug.Log("one");
		string[] temp = line.Split (new char[]{ ' ' });
		Node_data node = new Node_data ();
		node.time = float.Parse (temp [1]);
		node.x = float.Parse (temp [2]);
		node.y = float.Parse (temp [3]);
		node.z = float.Parse (temp [4]);

		nodeList.Add (node);
	}

	IEnumerator CallCamReset ()
	{
		yield return new WaitForSeconds (0.5f);
		cam.ResetCam ();
	}

	IEnumerator RemoveBubble ()
	{
		yield return new WaitForSeconds (3);
		if (ID.bubble != null) {
			ID.bubble.SetActive (false);
		}
	}
}
