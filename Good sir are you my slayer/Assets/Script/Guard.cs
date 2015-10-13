using UnityEngine;
using System.Collections;
using Database;

public class Guard : MonoBehaviour {

	public Player Target;
	public CharacterController Character;
	public int Health, Supsicion;
	public float Speed;

	void Update () 
	{
		if (Target != null) 
		{
			Target.IsWanted = true;
			float distance = Vector3.Distance(Target.transform.position,transform.position);
			if (distance > 1f)
				Character.Move (Vector3.MoveTowards(transform.position,Target.transform.position, 5f) * Speed * Time.deltaTime);
		}
	}
}
