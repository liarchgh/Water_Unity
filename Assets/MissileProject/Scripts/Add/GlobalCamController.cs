using UnityEngine;
using System.Collections;

public class GlobalCamController : MonoBehaviour {
	public Vector3 start_point;

	// Use this for initialization
	void Start () {
		start_point = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
		{
			transform.Translate(Vector3.forward * Time.deltaTime * 30f,Space.World);
		}
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
		{
			transform.Translate(Vector3.back * Time.deltaTime * 30f,Space.World);
		}
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{
			transform.Translate(Vector3.left * Time.deltaTime * 30f,Space.World);
		}
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		{
			transform.Translate(Vector3.right * Time.deltaTime * 30f,Space.World);
		}
		if (Input.GetKey(KeyCode.Escape))
		{
				transform.position = start_point;


		}
	}

}
