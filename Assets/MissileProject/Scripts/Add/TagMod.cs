using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class TagMod : MonoBehaviour {
	public List<Transform> trans;
	public List<string> name;
	public Text t;
	public bool NeedToHide=true;
	// Use this for initialization
	void Start () {
		trans=new List<Transform>();
		name=new List<string>();
		if(t==null)
			t=this.GetComponent<Text>();
		ClearEntry();
		if(NeedToHide)
			this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		t.text="";
		for(int i=0;i<trans.Count;i++)
		{
			t.text+=name[i]+(trans[i].position*10).ToString()+"\n";
		}
	}
	public void AddEntry(Transform temp,string n)
	{
		trans.Add(temp);
		name.Add(n);
	}
	public void SetEntry(Transform temp,string n)
	{
		ClearEntry();
		AddEntry(temp,n);
	}
	public void ClearEntry()
	{
		trans.Clear();
		name.Clear();
	}
}
