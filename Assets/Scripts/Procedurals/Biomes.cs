using UnityEngine;

public class Biome
{
    Vector3 location;
    public Vector3 Location {
        get {
            return location;
        }
    }
    int biomeIndex;
    public int BiomeIndex {
        get {
            return biomeIndex;
        }
    }

    public Biome(Vector3 location, int biomeIndex)
    {
        this.location = location;
        this.biomeIndex = biomeIndex;
    }
}