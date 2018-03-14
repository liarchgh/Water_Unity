using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ColoredBtn : MonoBehaviour {
	public Image img;
	public bool isTracked;
	public int x,y;
	public bool[,] data;
	public Text t;
	// Use this for initialization
	public void Init (bool b,int xx,int yy,bool[,] ddata,string name) {
		img=this.GetComponent<Image>();
		isTracked = b;
		ChangeColor ();
		x = xx;
		y = yy;
		data = ddata;
		t.text = name;

	}
	void ChangeColor()
	{
		if (isTracked) {
			img.color = Color.white;
		} else {
			img.color = Color.grey;
		}
	}
	public void OnPressed()
	{
		isTracked=!isTracked;
		data [x, y] = isTracked;
		ChangeColor ();
	}
}
