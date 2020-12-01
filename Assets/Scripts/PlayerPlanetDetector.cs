using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlanetDetector : MonoBehaviour
{
    GameController stats;
    private void Awake()
    {
        stats = GameController.instance;
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "TheSurface")
        {
            stats.NearAPlanet = true;
            stats.NearPlanet = col.gameObject.GetComponentInParent<GravityController>();
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "TheSurface")
        {
            stats.NearAPlanet = false;
            stats.NearPlanet = null;
        }
    }
}
