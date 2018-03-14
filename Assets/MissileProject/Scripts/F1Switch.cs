using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F1Switch : MonoBehaviour {
	public GameObject panel;
	public bool isActive=true;
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F1)) {
			isActive = !isActive;
			if (panel != null) {
				panel.SetActive (isActive);
			}
		}
	}
}
