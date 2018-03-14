using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ModeSettings : MonoBehaviour {
	public ParamSettings next;
	public void OnLocal()
	{
		Database.type = 1;
		next.gameObject.SetActive (true);
		next.Init ();
		this.gameObject.SetActive (false);
	}
	public void OnNet()
	{
		Database.type = 0;
	}
}
