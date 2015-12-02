using UnityEngine;
using System.Collections;
using Database;

public class Smoking_Area : MonoBehaviour
{
    void OnTriggerStay(Collider col)
    {
        Player player = col.GetComponent<Player>();
        if (Input.GetButton("X"))
        {
            if (player != null)
            {
                player.Needs[1].Meter = Mathf.Min(player.Needs[1].Meter+1,100);
            }
        }
    }
}
