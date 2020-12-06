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
        if (terrains == null)
        {
            terrains = new TerrainBiome[6];
            for (int i = 0; i < 6; i++)
            {
                //terrains[i] = new TerrainBiome(settings.density);
            }
        }
        else 
        {
            for (int i = 0; i < 6; i++)
            {
                //terrains[i].Reset();
            }
        }
    }
}
