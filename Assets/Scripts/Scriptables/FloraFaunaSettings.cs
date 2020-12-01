using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FloraFaunaSettings : ScriptableObject
{
    public GameObject[] biomeObjects;
    public int density;
    public TerrainBiome[] terrainBiome = new TerrainBiome[6];
}
