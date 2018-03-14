using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTagMod : MonoBehaviour {
	public string name;
	public TextMesh t;
	public NavObjIdentity ID;
	// Use this for initialization
	void Start () {
		t=this.GetComponent<TextMesh>();
	}

	// Update is called once per frame
	//void Update () {
		//t.text="";
		//t.text+=name+trans.position.ToString();
		//t.text+=name;

	//}
	public void ChangeText(string s)
	{
		t.text = s;
	}
}
