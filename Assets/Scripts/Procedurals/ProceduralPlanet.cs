using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProceduralPlanet : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 10;
    public bool autoUpdate = true;
    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };
    public FaceRenderMask faceRenderMask;

    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;
    public FloraFaunaSettings ffSettings;

    [HideInInspector]
    public bool shapeSettingsFoldout;
    [HideInInspector]
    public bool colorSettingsFoldout;
    [HideInInspector]
    public bool ffSettingsFoldout;

    ShapeGenerator shapeGenerator = new ShapeGenerator();
    ColorGenerator colorGenerator = new ColorGenerator();
    FloraFaunaGenerator ffGenerator = new FloraFaunaGenerator();

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilter;
    MeshFilter[] atmoMeshFilter;

    TerrainFace[] terrainFaces;
    TerrainBiome[] terrainBiomes;
    AtmosphereFace[] atmoFaces;

    GameObject biomesContainer;
    bool hasAtmosphere;

    private void Awake()
    {
        GeneratePlanet();   
    }
    void Initialized()
    {
        hasAtmosphere = shapeSettings.hasAtmosphere;

        shapeGenerator.UpdateSettings(shapeSettings);
        colorGenerator.UpdateSettings(colorSettings);
        ffGenerator.UpdateSettings(ffSettings);

        if (meshFilter == null || meshFilter.Length == 0)
        { 
            meshFilter = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];
        terrainBiomes = new TerrainBiome[6];

        ClearBiomes();

            if (atmoMeshFilter == null || atmoMeshFilter.Length == 0)
            {
                atmoMeshFilter = new MeshFilter[6];
            }
            atmoFaces = new AtmosphereFace[6];

        Vector3[] direction = { 
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back,
        };

        for (int i = 0; i < 6; i++)
        {
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            InitializeMesh(direction, i, renderFace);
            InitializeBiomes(i);

            if (hasAtmosphere)
            {
                InitializeAtmosphere(direction, i, renderFace);
            }
        }
    }
    private void InitializeMesh(Vector3[] direction, int i, bool renderFace)
    {
        if (meshFilter[i] == null)
        {
            GameObject meshObj = new GameObject("mesh " + i.ToString());
            meshObj.transform.parent = transform;
            meshObj.transform.position = transform.position;

            meshObj.tag = "TheSurface";
            meshObj.layer = LayerMask.NameToLayer("TheSurface");

            meshObj.AddComponent<MeshRenderer>();
            meshObj.AddComponent<MeshCollider>();

            meshFilter[i] = meshObj.AddComponent<MeshFilter>();
            meshFilter[i].sharedMesh = new Mesh();
        }
        meshFilter[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;

        terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilter[i].sharedMesh, resolution, direction[i], transform, ffGenerator, i);

        meshFilter[i].GetComponent<MeshCollider>().sharedMesh = meshFilter[i].sharedMesh;
        meshFilter[i].gameObject.SetActive(renderFace);
    }
    void InitializeBiomes(int i)
    {
        terrainBiomes[i] = new TerrainBiome(ffSettings, meshFilter[i].sharedMesh);
    }
    private void InitializeAtmosphere(Vector3[] direction, int i, bool renderFace)
    {
        if (atmoMeshFilter[i] == null)
        {
            GameObject atmoMeshObj = new GameObject("atmo mesh " + i.ToString());
            atmoMeshObj.transform.parent = transform;
            atmoMeshObj.transform.position = transform.position;

            atmoMeshObj.AddComponent<MeshRenderer>();

            atmoMeshFilter[i] = atmoMeshObj.AddComponent<MeshFilter>();
            atmoMeshFilter[i].sharedMesh = new Mesh();
        }

        atmoMeshFilter[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.atmosphereMaterial;
        atmoFaces[i] = new AtmosphereFace(shapeGenerator, atmoMeshFilter[i].sharedMesh, resolution, direction[i], i);

        atmoMeshFilter[i].gameObject.SetActive(renderFace);
    }
    public void GeneratePlanet()
    {
        Initialized();
        GenerateMesh();
        GenerateColors();
        GenerateBiomes();
    }
    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialized();
            GenerateMesh();
        }
    }
    public void OnColorSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialized();
            GenerateColors();
        }
    }
    public void OnFFSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialized();
            GenerateBiomes();
        }
    }
    private void GenerateMesh()
    {
        for (int i = 0; i < 6; i++)
        {
            if (meshFilter[i].gameObject.activeSelf)
            {
                terrainFaces[i].ConstructMesh();
                if (hasAtmosphere) atmoFaces[i].ConstructMesh();
            }
        }

        // pass elevation min max values to shader thru ColorGenerator class
        colorGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }
    private void GenerateColors()
    {
        colorGenerator.UpdateColors();
        colorGenerator.UpdateCloudColors();
        for (int i = 0; i < 6; i++)
        {
            if (meshFilter[i].gameObject.activeSelf)
            {
                terrainFaces[i].UpdateUVs(colorGenerator);
                if (hasAtmosphere) atmoFaces[i].UpdateUVs(colorGenerator);
            }
        }
    }
    void GenerateBiomes()
    {
        ClearBiomes();

        for (int i = 0; i < 6; i++)
        {
            terrainBiomes[i].PopulateBiome();

            Biome[] biomes = terrainBiomes[i].Biomes();

            for (int j = 0; j < biomes.Length; j++)
            {
                var block = Instantiate(
                    ffSettings.biomeObjects[biomes[j].BiomeIndex],
                    transform.position,
                    Quaternion.FromToRotation(Vector3.up, biomes[j].Location)
                );

                // random facing
                block.transform.Rotate(0f, Random.Range(0, 90), 0f, Space.Self);
                // random scaling
                block.transform.localScale *= Random.Range(0.8f, 2f);
                // align it to surface
                var footToCore = block.GetComponent<GravityBodyController>().foot.transform.position - transform.position;
                block.transform.position += (footToCore - biomes[j].Location) * -1;

                // put the object under moon gravity
                block.GetComponent<GravityBodyController>().AttachBody(this.GetComponent<GravityController>());
                block.transform.SetParent(biomesContainer.transform);
            }
        }
    }
    private void ClearBiomes()
    {
        if (biomesContainer != null)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(biomesContainer.gameObject);
            }
            else
            {
                Destroy(biomesContainer.gameObject);
            }
            biomesContainer = null;
        }
        biomesContainer = new GameObject("biomes " + gameObject.name);
        biomesContainer.transform.SetParent(transform);
    }
}
