using UnityEngine;  
using System.Collections;  
using System.Diagnostics;  
using System;  
using System.IO;
public class VideoCapturer : MonoBehaviour {
	public static void CreateMovie(string path,string comName,string outFileName) {  
		Process p = new Process();  
		string curDir = System.Environment.CurrentDirectory;
		#if UNITY_STANDALONE_WIN
		p.StartInfo.FileName = curDir + "/ffmpeg.exe";
		#else
		p.StartInfo.FileName = curDir + "/ffmpeg";
		#endif
		//print(System.Environment.CurrentDirectory + @"/ffmpeg.exe");  
		string arg = string.Format("-r 25 -i {0}/{1}/{2}%d.jpg {3}", curDir,path, comName, outFileName);  
		p.StartInfo.Arguments = arg;//参数  
		p.StartInfo.UseShellExecute = false;//不使用操作系统外壳程序启动  
		p.StartInfo.RedirectStandardError = true;//重定向标准错误流  
		p.StartInfo.CreateNoWindow = true;  
		p.ErrorDataReceived += new DataReceivedEventHandler(Output);//输出流的事件  
		p.Start();//启动  
		p.BeginErrorReadLine();//开始异步读取  
		p.WaitForExit();//阻塞等待进程结束  
		p.Close();  
		p.Dispose();  
		if (OnMovieCreateOver != null) {  
			OnMovieCreateOver(outFileName);  
		}  
	}  
	private static void Output(object sendProcess, DataReceivedEventArgs output) {  
		if (!String.IsNullOrEmpty(output.Data)) {  
			UnityEngine.Debug.Log(output.Data);  
		}  
	}  
	int state = -1;  
	int rate = 25;  

	private static VideoCapturer inst;  
	public static VideoCapturer Inst {  
		get { return inst; }  
	}  
	public static Action<string> OnMovieCreateOver;  
	void Awake() {  
		inst = this;  
		if (!Directory.Exists("tImg"))  
			Directory.CreateDirectory("tImg");  
		if (!Directory.Exists("movie"))  
			Directory.CreateDirectory("movie");  
		curSpan = 1.0f / 25;  

		StartCoroutine(Loop());  
	}  
	public void StartRecord() {  
		state = 1;  
		Directory.Delete("tImg", true);  
		Directory.CreateDirectory("tImg");  
	}  
	//public void PauseRecord() {  
	//    state = 0;  
	//}  
	public void StopRecord() {  
		state = -1;  
		index = 0;  
		CreateMovie("tImg", "img", string.Format("movie/{0}.mp4",DateTime.Now.ToString("yyyyMMddhhmmss")));  
	}  
	public void SetRate() {   
	}  
	float curSpan = 0;  
	float nowTime = 0;  
	int index = 0;  
	IEnumerator Loop() {  
		while (true) {  
			yield return new WaitForEndOfFrame();  
			if (state == 1) {  
				nowTime += Time.deltaTime;  
				if (nowTime > curSpan) {  
					nowTime = 0;  
					Texture2D t = capTex();  
					//t.Apply();  
					SaveJpg(t, "tImg/img", index);  
					index++;  
					Resources.UnloadUnusedAssets();  
				}  
			}  
		}  
	}  

	Texture2D capTex() {  
		Texture2D t = new Texture2D(Screen.width, Screen.height);  
		t.ReadPixels(new Rect(0,0,Screen.width,Screen.height), 0, 0);  
		return t;  
	}  
	void SaveJpg(Texture2D tex,string comName,int index) {  
		var bytes = tex.EncodeToJPG();  
		FileStream fs = new FileStream(comName+index+".jpg",FileMode.Create);  
		fs.Write(bytes, 0, bytes.Length);  
		fs.Flush();  
		fs.Close();  
	}  
}  