using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
/*public struct Node_data
{
	public float time;
	public float x,y,z;
}*/
public class Mover : MonoBehaviour {
	public bool isUnderwater=true,HasBubble=false;
	public GameObject pointer,bubble;
	public CameraIdentity cam,thiscam;
	public ArrayList arrlist;
	public List<Node_data> nodeList;
	public float interv,dt=0.1f,scaleFactor=0.1f;
	public int ptr;
	public Transform trans;
	public string FileName="position.txt";
	public string FilePath="";
    public Replay rep_timer;
	// Use this for initialization
	void Start () {
		if (thiscam != null) {
			FileName = thiscam.fileName;
			FilePath = thiscam.filePath;
		}
		trans=Instantiate(pointer).transform;
		if (FilePath.Equals ("")) {
			LoadFile (Application.streamingAssetsPath + '/' + FileName);
		} else {
			LoadFile (FilePath + '/' + FileName);
		}
		ptr=0;
        rep_timer = GameObject.Find("MyTimer").GetComponent<Replay>();
        rep_timer.AddRepObj(this);
		InvokeRepeating("UpdatePos",0,dt);
	}

    // Update is called once per frame
    void UpdatePos()
    {
        if (ptr == 0)
        {
            //Reset Trail
            //thiscam.trail.Clear();
            //thiscam.trailMap.Clear();
            if (thiscam!=null)
            {
                if (thiscam.isTrackedObj)
                {
                    thiscam.line.positionCount = 0;
                    thiscam.lineMap.positionCount = 0;
                }
            }
        }
        //		if(ptr<nodeList.Count)
        //		{
        //			Debug.Log("updatepos"+Time.realtimeSinceStartup);
        //			while(nodeList[ptr].time<Time.realtimeSinceStartup&&ptr<nodeList.Count-1)
        //			{
        //			ptr++;
        //
        //			}
        //			transform.position=new Vector3(nodeList[ptr].x,nodeList[ptr].y,nodeList[ptr].z);		trans.LookAt(transform);
        //			Debug.Log(transform.position);
        //		}



        if (ptr < nodeList.Count - 1)
        {
            if (isUnderwater)
            {
                while (nodeList[ptr].time*0.25 < rep_timer.currentTime && ptr < nodeList.Count - 1)//TODO:remove *0.25
                {
                    transform.position = new Vector3(nodeList[ptr].x, nodeList[ptr].z - 10f / 0.03f, nodeList[ptr].y) * scaleFactor;
                    if (thiscam != null)
                    {
                        if (thiscam.isTrackedObj)
                        {
                            thiscam.line.positionCount++;
                            thiscam.line.SetPosition(ptr, new Vector3(nodeList[ptr].x, nodeList[ptr].z - 10f / 0.03f, nodeList[ptr].y) * scaleFactor);
                            thiscam.lineMap.positionCount++;
                            thiscam.lineMap.SetPosition(ptr, new Vector3(nodeList[ptr].x, nodeList[ptr].z - 10f / 0.03f, nodeList[ptr].y) * scaleFactor);
                        }
                    }
                    trans.position = new Vector3(nodeList[ptr + 1].x, nodeList[ptr + 1].z - 10f / 0.03f, nodeList[ptr + 1].y) * scaleFactor;
                    transform.LookAt(trans);
                    ptr++;
                }
            }
            else
            {
                if (thiscam != null)
                {
                    if (thiscam.isTrackedObj)
                    {
                        thiscam.line.positionCount++;
                        thiscam.line.SetPosition(ptr, new Vector3(nodeList[ptr].x, nodeList[ptr].z - 10f / 0.03f, nodeList[ptr].y) * scaleFactor);
                        thiscam.lineMap.positionCount++;
                        thiscam.lineMap.SetPosition(ptr, new Vector3(nodeList[ptr].x, nodeList[ptr].z - 10f / 0.03f, nodeList[ptr].y) * scaleFactor);
                    }
                }
                transform.position = new Vector3(nodeList[ptr].x, transform.position.y / scaleFactor, nodeList[ptr].y) * scaleFactor;
                trans.position = new Vector3(nodeList[ptr + 1].x, trans.position.y / scaleFactor, nodeList[ptr + 1].y) * scaleFactor;
                //transform.LookAt(trans);
                ptr++;
            }

            //Debug.Log(transform.position);
           
            if (ptr >= nodeList.Count - 1)
            {
                StartCoroutine(RemoveBubble());
            }
        }
        if (ptr == 1)
        {
            if (cam != null)
                cam.ResetCam();
            if (FileName.Equals("position1.txt"))
            {
                CameraIdentity tmp = GameObject.Find("FirstPersonCharacter").GetComponent<CameraIdentity>();
                tmp.ResetCam();
            }


        }





/*//old version
        if (ptr<nodeList.Count-1)
		{
			if(isUnderwater)
			{
				transform.position=new Vector3(nodeList[ptr].x,nodeList[ptr].z-10f/0.03f,nodeList[ptr].y)*scaleFactor;
				trans.position=new Vector3(nodeList[ptr+1].x,nodeList[ptr+1].z-10f/0.03f,nodeList[ptr+1].y)*scaleFactor;
				transform.LookAt(trans);
			}else{
				transform.position=new Vector3(nodeList[ptr].x,transform.position.y/scaleFactor,nodeList[ptr].y)*scaleFactor;
				trans.position=new Vector3(nodeList[ptr+1].x,trans.position.y/scaleFactor,nodeList[ptr+1].y)*scaleFactor;
				//transform.LookAt(trans);
			}

			//Debug.Log(transform.position);
			ptr++;
			if(ptr>=nodeList.Count-1)
			{
				StartCoroutine(RemoveBubble());
			}
		}
		if(ptr==1)
		{
			if(cam!=null)
				cam.ResetCam();
			if(FileName.Equals("position1.txt"))
			{
				CameraIdentity tmp=GameObject.Find("FirstPersonCharacter").GetComponent<CameraIdentity>();
				tmp.ResetCam();
			}
		}*/
	}
	void LoadFile(string name)
	{

		arrlist=new ArrayList();
		nodeList=new List<Node_data>();
		try
		{
			// Create an instance of StreamReader to read from a file.
			// The using statement also closes the StreamReader.
			using (StreamReader sr = new StreamReader(name)) 
			{
				String line;
				// Read and display lines from the file until the end of 
				// the file is reached.
				while ((line = sr.ReadLine()) != null) 
				{
					//Debug.Log("Scan");
					arrlist.Add(line);
					ScanFile(line);
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
	void ScanFile(string line)
	{
		//Debug.Log("one");
		string[] temp=line.Split(new char[]{' '});
		Node_data node=new Node_data();
		node.time=float.Parse(temp[1]);
		node.x=float.Parse(temp[2]);
		node.y=float.Parse(temp[3]);
		node.z=float.Parse(temp[4]);

		nodeList.Add(node);
	}
	IEnumerator RemoveBubble()
	{
		yield return new WaitForSeconds(3);
		if(HasBubble&&bubble!=null)
		{
			bubble.SetActive(false);
		}
	}
}
