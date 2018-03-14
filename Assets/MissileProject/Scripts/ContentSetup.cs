using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ContentSetup : MonoBehaviour {
	public ColoredBtn originalBtn;
	public RectTransform rectPanel;
	public Vector3 startPos,currentPos;
	public float intervY=-40;
	public void SetupBoolTable(int[] initCount,bool[,]initTracked)
	{
		bool isEmpty = true;
		currentPos = startPos;
		for (int i = 0; i < initCount.Length; i++) {
			for (int j = 0; j < initCount [i]; j++) {
				isEmpty = false;
				ColoredBtn tmp=Instantiate (originalBtn).GetComponent<ColoredBtn>();
				tmp.transform.SetParent (rectPanel.transform);
				tmp.GetComponent<RectTransform> ().localPosition = new Vector3(196+currentPos.x,currentPos.y,currentPos.z);   //????
				if (currentPos.x > 0) {
					currentPos.y += intervY;
				}
				currentPos.x = -currentPos.x;
				string name="";
				if (i == 0) {
					name = "T" + (j+1);
				} else if (i == 1) {
					name = (j+1)+"#";
				} else if (i == 2) {
					name = "潜艇" + (j+1);
				} else if (i == 3) {
					name = "实验船" + (j+1);
				} else if (i == 4) {
					name = "应答器" + (j+1);
				}
				tmp.Init (initTracked [i, j],i,j,initTracked,name);
			}
			if (currentPos.x > 0) {
				currentPos.y += intervY;
			}
			currentPos.x = startPos.x;
			if(!isEmpty)
				currentPos.y = currentPos.y+intervY*0.5f;
			isEmpty = true;
		}
		rectPanel.sizeDelta = new Vector2 (rectPanel.sizeDelta.x, -(currentPos.y-10));
	}
}
