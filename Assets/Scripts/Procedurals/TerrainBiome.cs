using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainBiome
{
    FloraFaunaSettings settings;
    Mesh mesh;

    Biome[] biomes;

    public TerrainBiome(FloraFaunaSettings settings, Mesh mesh)
    {
        this.settings = settings;
        this.mesh = mesh;

        biomes = new Biome[settings.density];
    }
    public void PopulateBiome()
    {
        Vector3[] vertices = mesh.vertices;

        List<int> filled = new List<int>();
        int pickedVertice = 0;
        int pickedObjIndex = 0;

        for (int i = 0; i < biomes.Length; i++)
        {
            pickedVertice = (int)Random.Range(0, vertices.Length);
            if (filled.Contains(pickedVertice)) continue;

            pickedObjIndex = (settings.biomeObjects.Length > 1) ? (int)Random.Range(0, settings.biomeObjects.Length) : 0;

            Biome biome = new Biome(vertices[pickedVertice], pickedObjIndex);

            biomes[i] = biome;
        }
    }
    public Biome[] Biomes()
    {
        return biomes;
    }
}