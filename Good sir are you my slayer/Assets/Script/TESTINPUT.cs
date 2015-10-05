using UnityEngine;
using System.Collections;

public class TESTINPUT : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Translate (Vector3.right * Input.GetAxis ("Horizontal"));
			//print (transform.position.x);
		transform.Translate (Vector3.up * Input.GetAxis ("Vertical"));
	//	print (transform.position.x);

		if (Input.GetButtonDown("X"))
		    print ("X");
		if (Input.GetButtonDown("B"))
			print ("B");
		if (Input.GetButtonDown("A"))
			print ("A");
		if (Input.GetButtonDown("Y"))
			print ("Y");
		if (Input.GetButtonDown("LBumper"))
			print ("LBumper");
		if (Input.GetButtonDown("RBumper"))
			print ("RBumper");
		if (Input.GetButtonDown("Start"))
			print ("Start");
		if (Input.GetButtonDown("Select"))
			print ("Select");
		transform.Translate (Vector3.right * Input.GetAxis ("DPad X"));
		transform.Translate (Vector3.up * Input.GetAxis ("DPad Y"));

		transform.Translate (Vector3.right * Input.GetAxis ("RBumper"));
		transform.Translate (Vector3.up * Input.GetAxis ("LBumper"));
	}
}
