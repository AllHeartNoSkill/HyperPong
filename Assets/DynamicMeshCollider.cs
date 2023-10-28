using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicMeshCollider : MonoBehaviour
{

    SkinnedMeshRenderer meshRenderer;
    MeshCollider collider;

    private bool isRebuilding = true;

    IEnumerator RebuildMesh() {
        while (isRebuilding) {
            yield return new WaitForSeconds(0.2f);
            Rebuild();
        }
    }


    // Start is called before the first frame update
    void Start()
    {

        meshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
        collider = gameObject.GetComponent<MeshCollider>();
        // StartCoroutine(RebuildMesh());
    }

    // Update is called once per frame
    void Update()
    {
        Rebuild();
    }

    void Rebuild() {
        Mesh colliderMesh = new Mesh();
        meshRenderer.BakeMesh(colliderMesh);
        collider.sharedMesh = null;
        collider.sharedMesh = colliderMesh;
    }


}
