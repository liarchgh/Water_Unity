
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Replay : MonoBehaviour
{
	public Slider s;
	public Text text_RepBtn, text_RecordBtn;
	public AVProMovieCaptureFromCamera MovieBase;
	public AVProMovieCaptureGUI MovieGUI;
	public float time_since_started;
	public float currentTime;
	public bool isRepMode = false, isDragging = false, isStop = false, isRecording = false;
	public List<Mover> movers;
	// Use this for initialization
	void Start ()
	{
		currentTime = 0;
		movers = new List<Mover> ();
	}

	public void AddRepObj (Mover m)
	{
		movers.Add (m);
	}
	// Update is called once per frame
	void Update ()
	{
		if (MovieGUI != null) {
			if (Input.GetKeyDown (KeyCode.F9)) {
				MovieGUI.enabled = !MovieGUI.enabled;
			}
		}
		time_since_started = Time.time;
		if (isRepMode) {
			if (!isDragging) {
				if (!isStop) {
					currentTime += Time.deltaTime;
				}
				s.value = currentTime / time_since_started;
			}
		} else {
			currentTime = time_since_started;
		}
        
	}

	public void OnValueChanged (float value)
	{
        
	}

	public void OnPointerDown ()
	{
		isDragging = true;
	}

	public void OnPointerUp ()
	{
		if (s.value != 1) {
			currentTime = time_since_started * s.value;
			if (isRepMode != true) {
                
				isRepMode = true;
			}

		} else {
			if (isRepMode != false)
				isRepMode = false;
		}
		foreach (Mover m in movers) {
			m.ptr = 0;
		}
		isDragging = false;
	}

	public void OnChangeState ()
	{
		if (isStop) {
			isStop = false;
			text_RepBtn.text = "暂停播放";
		} else {
			isStop = true;
			if (!isRepMode) {
				isRepMode = true;
			}
			text_RepBtn.text = "开始播放";
		}
	}

	public void OnChangeRecord ()
	{
		if (isRecording) {
			MovieBase.StopCapture ();
			text_RecordBtn.text = "开始录制";

		} else {
			MovieBase.StartCapture ();
			text_RecordBtn.text = "停止录制";
		}
		isRecording = !isRecording;
	}
	public void OnChangeRecord2()
	{
		if (isRecording) {
			VideoCapturer.Inst.StopRecord ();
			text_RecordBtn.text = "开始录制";

		} else {
			VideoCapturer.Inst.StartRecord ();
			text_RecordBtn.text = "停止录制";
		}
		isRecording = !isRecording;
	}
}
