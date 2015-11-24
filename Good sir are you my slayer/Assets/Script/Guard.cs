using UnityEngine;
using System.Collections;
using Database;

public class Guard : MonoBehaviour {

	public Player Target;
    public Unit Unit;
    public LayerMask layermask;

	void Update () 
	{
		if (Target != null) 
		{
			Target.IsWanted = true;
            print("I AM GUARDING AGAINST YOU");
            bool walkable = (Physics.CheckSphere(Target.transform.position, 0.5f, layermask));
            if (walkable)
                Unit.MoveTo(Target.transform.position);
            float distance = Vector3.Distance(Target.transform.position, transform.position);
            if (distance < 1f)
            {
                Unit.MoveTo(Target.transform.position);
                Target.Health = 0;
            }
        }
	}
}
