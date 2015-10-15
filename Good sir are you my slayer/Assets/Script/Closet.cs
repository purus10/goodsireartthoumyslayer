using UnityEngine;
using System.Collections;
using Database;

public class Closet : MonoBehaviour {
	
	public Sprite[] Head;
	public Sprite[] Body;
	
	public Sprite GetHead { get { return Head [Random.Range (0, Head.Length)]; } }
	public Sprite GetBody { get { return Body [Random.Range (0, Body.Length)]; } }

	bool NotLikeTarget(Sprite head, Sprite body)
	{
		bool pass = false;
		if (head != Get.TargetHead)
			pass = true;
		else if (body != Get.TargetBody)
			pass = true;
		else pass = false;
		return pass;
	}

	public void DressUp(Player player)
	{
		Sprite PreBody = player.Body.sprite;
		Sprite PreHead = player.Head.sprite;
		PlaceSprites (player);
		//StartCoroutine ("DressSet (player)");

	}
	void PlaceSprites(Player player)
	{
		player.Body.sprite = GetBody;
		player.Head.sprite = GetHead;
	}

	IEnumerator DressSet(Player player)
	{
		PlaceSprites (player);
		Sprite Body = player.Body.sprite;
		Sprite Head = player.Head.sprite;
		if (NotLikeTarget (Body,Head) == false)
		yield return null;
	}

	void Update()
	{
		transform.Translate( Vector3.up * 1f * Time.deltaTime);
	}
	void OnCollisionEnter () 
	{
		print ("Change your face! :D");
	}


}
