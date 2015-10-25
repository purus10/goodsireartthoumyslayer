using UnityEngine;
using System.Collections;

public class Result : MonoBehaviour {

	static public bool End;
	static public string[] PlayerName = new string[4];
	public Rect ResultBox;
	public Rect[] PlayerDisplay = new Rect[4];
	public Rect NextRound;
	
	// Update is called once per frame
	void OnGUI () 
	{
		if (End)
		{
		GUI.Box(ResultBox,"");

		}
	}
}
