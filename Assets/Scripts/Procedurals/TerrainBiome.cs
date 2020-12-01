using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBiome
{
    public Biome[] biomes;
    int density;

    public TerrainBiome(int density)
    {
        this.density = density;
        biomes = new Biome[density];
    }
    [System.Serializable]
    public class Biome
    {
        Vector3 location;
        public Vector3 Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }
        int biomeIndex;
        public int BiomeIndex
        {
            get
            {
                return biomeIndex;
            }
            set
            {
                biomeIndex = value;
            }
        }
        GameObject biomeObject;
        public GameObject BiomeObject
        {
            get 
            {
                return biomeObject;
            }
            set 
            {
                biomeObject = value;
            }
        }
        public Biome(Vector3 l, int objIndex)
        {
            location = l;
            biomeIndex = objIndex;
        }
    }
    public void AddBiome(int index, Vector3 l, int objIndex)
    {
        Biome b = new Biome(l, objIndex);
        biomes[index] = b;
    }
    public void Reset()
    {
        this.density = density;
        biomes = new Biome[density];
        if (biomes != null && biomes.Length > 0)
        {
            for (int i = 0; i < biomes.Length; i++)
            {
                if (biomes[i].BiomeObject)
                {
                    try
                    {
                        biomes[i].BiomeObject.gameObject.GetComponent<GravityBodyController>().destroySelf();
                    }
                    catch(Exception e)
                    {  }
                }
            }
        }
    }
}