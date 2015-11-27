using UnityEngine;
using System.Collections;
using Database;

public class SetName : MonoBehaviour {

	public TextMesh text;
    public Player player;
	
	void Start () {
	
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        text.text = player.TargetName;
    }
}
