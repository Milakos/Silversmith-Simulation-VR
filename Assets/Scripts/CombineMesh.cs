using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Copy meshes from children into the parent's Mesh.
// CombineInstance stores the list of meshes.  These are combined
// and assigned to the attached Mesh.

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class CombineMesh : MonoBehaviour
{
    public List<MeshFilter> meshFilters;
    public MeshFilter targetmeshFilter;

    // Start is called before the first frame update
    [ContextMenu("Combine Meshes")]
    void CombineMeshes()
    {
        CombineInstance[] combine = new CombineInstance[meshFilters.Count];
        
        for (int i = 0; i < meshFilters.Count; i++)
        {    
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }
        
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine);
        targetmeshFilter.mesh = combinedMesh;
        print("CombineMesh");
    }
}
