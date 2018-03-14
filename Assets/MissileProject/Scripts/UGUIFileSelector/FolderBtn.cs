using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FolderBtn : BtnBase {
	public UGUISelectorBase selector;
	public bool isFirstClicked=false;
	public void Init(int n,string t,UGUISelectorBase ui)
	{
		txt.text=t;
		no=n;
		selector = ui;
	}
	public void OnPressed()
	{
		if(!isFirstClicked)
		{
			isFirstClicked=true;
			img.color=Color.blue;
			StartCoroutine(doubleClickCountdown());
			selector.Select (this);
		}else{
			if(no<0)
			{
				Debug.Log("Double");
				selector.Prev();
			}else{
				Debug.Log ("next");
				selector.Next (no);
			}
		}
	}
	IEnumerator doubleClickCountdown()
	{
		yield return new WaitForSeconds(0.5f);
		isFirstClicked=false;

	}
}
