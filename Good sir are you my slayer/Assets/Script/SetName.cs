using UnityEngine;
using System.Collections;
using Database;

public class SetName : MonoBehaviour {

	public TextMesh text;
    public Player player;
	
	void Start () {
        GetComponent<MeshRenderer>().sortingOrder = 3;

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        text.text = player.TargetName;

    }
}
