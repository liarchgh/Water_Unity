using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 5F;
	public float sensitivityY = 5F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;
    public GameObject StartPot;

	public Camera cam;
	float rotationY = 0F;
	void Start()
	{
		cam=GameObject.Find("FirstPersonCharacter").GetComponent<Camera>();
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}
	void Update ()
	{
		if(cam.isActiveAndEnabled)
		{
		if(Input.GetMouseButton(1))
		{
			if (axes == RotationAxes.MouseXAndY)
			{
				float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
				
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
			}
			else if (axes == RotationAxes.MouseX)
			{
				transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
			}
			else
			{
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
			}
		}

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * 3f);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.back * Time.deltaTime * 3f);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * Time.deltaTime * 3f);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * Time.deltaTime * 3f);
        }
        if (Input.GetKey(KeyCode.Space))
        {
			transform.Translate(Vector3.up * Time.deltaTime * 3f,Space.World);
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
			transform.Translate(Vector3.down * Time.deltaTime * 3f,Space.World);

        }
        if (Input.GetKey(KeyCode.Escape))
        {
				ResetCam();

        }

		if (Input.GetAxis("Mouse ScrollWheel") <0)
		{
			if(cam.fieldOfView<=100)
				cam.fieldOfView +=2;
			if(cam.orthographicSize<=20)
				cam.orthographicSize +=0.5F;
		}
		//Zoom in
		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			if(cam.fieldOfView>2)
				cam.fieldOfView-=2;
			if(cam.orthographicSize>=1)
				cam.orthographicSize-=0.5F;
		}
		//Zoom out
		}
    }
	public void ResetCam()
	{
		if (StartPot != null)
		{
			transform.position = StartPot.transform.position;
			transform.rotation = StartPot.transform.rotation;
		}
	}

}