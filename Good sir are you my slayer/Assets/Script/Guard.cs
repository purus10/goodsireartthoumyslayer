using UnityEngine;
using System.Collections;
using Database;
using UnityEngine.Networking;

public class Guard : NetworkBehaviour {

	public Player Target;
    public Vector3 StartPoint;
    public Unit Unit;
    public LayerMask layermask;

    void Start()
    {
        StartPoint = transform.position;
    }

    void OnTriggerStay(Collider col)
    {
        Npc npc = col.gameObject.GetComponentInParent<Npc>();

        if (npc != null && npc.offender != null && npc.State == Npc.states.Afraid)
        {
            Target = npc.offender;
        }
    }

    void Update()
    {
        if (Target != null)
        {
            GetComponentInChildren<SpriteBubble>().Anim_Bubble(8, 11);
            Target.IsWanted = true;
            bool walkable = (Physics.CheckSphere(Target.transform.position, 0.5f, layermask));
            if (walkable)
                Unit.MoveTo(Target.transform.position);
            float distance = Vector3.Distance(Target.transform.position, transform.position);
            if (distance < 1.5f)
            {
                Target.Health = 0;
                Unit.MoveTo(StartPoint);
                Target = null;
            }
        }

        if (Target == null)
            GetComponentInChildren<SpriteBubble>().Bubble.sprite = GetComponentInChildren<SpriteBubble>().Bubbles[20];
    }
}
