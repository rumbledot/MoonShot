using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloraFaunaGenerator
{
    FloraFaunaSettings settings;
    public TerrainBiome[] terrains;

    public void UpdateSettings(FloraFaunaSettings settings)
    {
        this.settings = settings;
        if (terrains != null && terrains.Length > 0)
        {
            for (int i = 0; i < 6; i++)
            {
                terrains[i].Reset();
            }
        }
        terrains = new TerrainBiome[6];
        for (int i = 0; i < 6; i++)
        {
            terrains[i] = new TerrainBiome(settings.density);
        }
    }
    public void PopulateTerrain(Vector3[] vertices, int terrainIndex, Transform center)
    {
        TerrainBiome terrain = new TerrainBiome(settings.density);

        List<int> filled = new List<int>();

        if (settings.density > 0)
        { 
            int pickedVertice = 0;
            int pickedObjIndex = 0;

            for (int i = 0; i < settings.density; i++)
            {
                pickedVertice = (int)Random.Range(0, vertices.Length);
                if (!filled.Contains(pickedVertice))
                {
                    pickedObjIndex = (settings.biomeObjects.Length > 1) ? (int)Random.Range(0, settings.biomeObjects.Length) : 0;
                    terrain.AddBiome(i, vertices[pickedVertice] - center.position, pickedObjIndex);
                }
            }
            terrains[terrainIndex] = terrain;
        }
    }
}
