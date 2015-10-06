using UnityEngine;
using System.Collections;

public class Change : MonoBehaviour {

	public Sprite[] Head;
	public Sprite[] Body;

	public Sprite GetHead { get { return Head [Random.Range (0, Head.Length)]; } }
	public Sprite GetBody { get { return Body [Random.Range (0, Body.Length)]; } }

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
