using UnityEngine;
using System.Collections;

public class Toilet_Area : MonoBehaviour {

    void OnTriggerStay(Collider col)
    {
        Player player = col.GetComponent<Player>();
        if (Input.GetButtonDown("X"))
        {
            if (player != null)
            {
                player.Needs[3].Meter = 100;
            }
        }
    }
}
