using UnityEngine;
using System.Collections;

public class Change : MonoBehaviour {

	public Sprite[] Head;
	public Sprite[] Body;
	public Sprite[] Accessory;

	public Sprite GetHead { get { return Head [Random.Range (0, Head.Length)]; } }
	public Sprite GetBody { get { return Body [Random.Range (0, Body.Length)]; } }
	public Sprite GetAccessory { get { return Accessory [Random.Range (0, Accessory.Length)]; } }

	// Use this for initialization
	void Start () 
	{
		print ("moo");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
