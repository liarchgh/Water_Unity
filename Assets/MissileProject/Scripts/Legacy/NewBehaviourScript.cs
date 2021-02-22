using UnityEngine;
using System.Collections;
using Suimono.Core;
using System.Collections.Generic;

public class NewBehaviourScript : MonoBehaviour
{
    private Suimono_UnderwaterFog off;
    private Camera[] cama = new Camera[] { };
    public List<GameObject> list;
    void Start()
    {
        list = new List<GameObject>();
        off = GetComponent<Suimono_UnderwaterFog>();
        cama = Camera.allCameras;
        for (int i = 0; i < cama.Length; i++)
        {
            if (cama[i].gameObject.tag == "missile")
            {
                list.Add(cama[i].gameObject);
            }
        }
    }

    public void Conceal()
    {
        //Camera.main.rect = new Rect(0, 0, 1, 1);
        //foreach (var VARIABLE in list)
        //{
        //    VARIABLE.SetActive(false);
        //}
    }

    public void Show()
    {
        //Camera.main.rect = new Rect(0, 0, 1, 1F);
        foreach (var VARIABLE in list)
        {
            VARIABLE.SetActive(true);
        }
    }
}
