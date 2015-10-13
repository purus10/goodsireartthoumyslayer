using UnityEngine;
using System.Collections;

public class HUD_Suspicion : MonoBehaviour {

	public Player player;
	public SpriteRenderer Witnessed;
	public SpriteRenderer Wanted;

	void Update () 
	{
		Witnessed.enabled = player.IsSeen;
		Wanted.enabled = player.IsWanted;

	}
}
