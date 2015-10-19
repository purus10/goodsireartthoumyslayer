using UnityEngine;
using System.Collections;
using Database;

public class SetName : MonoBehaviour {

	public TextMesh text;
	
	void Start () {
	
		text.text = Get.TargetName;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
