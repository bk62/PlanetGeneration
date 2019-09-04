using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    [Range (2, 256)]
    public int resolution = 10;

    // serialize in order to be able to save
    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    private void OnValidate () {
        // update from editor
        Initialize ();
        GenerateMesh ();
    }

    void Initialize () {
        if (meshFilters == null || meshFilters.Length == 0) {
            // only re-initialize if uninitialized
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
        
        for (int i = 0; i < 6; i++) {
            if (meshFilters[i] == null) { 
                // only create mesh if ith mesh filter ininitialized
                GameObject meshObj = new GameObject ("mesh");
                meshObj.transform.parent = transform;

                // default material
                meshObj.AddComponent<MeshRenderer> ().sharedMaterial = new Material (Shader.Find ("Standard"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter> ();
                meshFilters[i].sharedMesh = new Mesh ();
            }

            // terrain face in each direction
            terrainFaces[i] = new TerrainFace (meshFilters[i].sharedMesh, resolution, directions[i]);
        }

    }

    void GenerateMesh () {
        foreach (TerrainFace face in terrainFaces) {
            face.ConstructMesh ();
        }
    }

}