using UnityEngine;
using System.Collections;

public class Rotater : MonoBehaviour {

	public float Speed;
    float Zper=1f;
    float ZTotle = 0f;

	void Update () {

        this.transform.Rotate(0f, 1f * Speed, 0f, Space.Self);
        //this.transform.Rotate(0f, 1f * Speed, -Zper * Speed, Space.Self);
        //ZTotle += Zper;
        //if (ZTotle > 15f)
        //{
        //    Zper = -Zper;
        //}
        //else if (ZTotle < -15f)
        //{
        //    Zper = -Zper;

        //}

    }
}
