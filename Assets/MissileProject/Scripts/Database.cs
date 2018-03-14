using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Database {
	public static int type = 0;
	public static float umiDepth=50;  //0 is net while 1 is local
	public static string filePath="";
	public static int[] initCount;
	public static bool[,] initTracked;
	public static Vector3 initCamPos;
	public static bool isSetInitCamPos=false;
	public static void initCountSet(int length,int[] data)
	{
		initCount = new int[length];
		for (int i = 0; i < length; i++) {
			initCount [i] = data [i];
		}
	}
	public static void initTrackedSet(int x,int y,bool[,] data)
	{
		initTracked = new bool[x,y];
		for (int i = 0; i < x; i++) {
			for (int j = 0; j < y; j++) {
				initTracked[i,j] = data [i, j];
			}
		}
	}
	public static void initCamPosSet(float x,float y,float z)
	{
		initCamPos=new Vector3(x,y,z);
	}
}
