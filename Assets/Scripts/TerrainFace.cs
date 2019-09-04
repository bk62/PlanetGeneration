using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace {
    // generate mesh in a given direction

    Mesh mesh;
    int resolution;
    Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;

    public TerrainFace (Mesh mesh, int resolution, Vector3 localUp) {
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;

        axisA = new Vector3 (localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross (localUp, axisA);
    }

    public void ConstructMesh () {
        // resolution^2 vertices
        Vector3[] vertices = new Vector3[resolution * resolution];
        // (resolution - 1)^2 squares, 2 triangles for each square and 3 vertices per triangle 
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 2 * 3];
        int triIndex = 0;

        for (int y = 0; y < resolution; y++) {
            for (int x = 0; x < resolution; x++) {
                int i = x + y * resolution; // resolution points per y loop
                // calc vertex position along the face usinng the percent along the axes from the face origin
                Vector3 percent = new Vector2 (x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                // normalized so each vertex is the same distance from planet origin
                // (uneven distribution of points)
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] = pointOnUnitSphere;

                // skip right and bottom edges
                if (x != resolution - 1 && y != resolution - 1) {
                    // triangles defined clockwise

                    // lower triangle in a square  (e.g. ACD if ABCD clockwise square)
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;

                    // upper triangle in a square (e.g. ABC if ABCD clockwise square)
                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;

                    triIndex += 6; // 6 triangle vertices added
                }
            }
        }

        // assigning vertices when lowering resolution causes error b/c some triangle indices are invalid 
        // so clear mesh first
        mesh.Clear ();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals ();
    }

}