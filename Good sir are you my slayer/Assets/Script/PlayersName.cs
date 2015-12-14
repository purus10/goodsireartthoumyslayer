using UnityEngine;
using System.Collections;

public class PlayersName : MonoBehaviour {

    public TextMesh Name;
    public Player player;
	
	// Update is called once per frame
	void Start ()
    {
        GetComponent<MeshRenderer>().sortingOrder = 10;
        Name.text = player.Name;
	}
}
