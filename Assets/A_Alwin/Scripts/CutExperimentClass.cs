using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutExperimentClass : MonoBehaviour
{
    public GameObject objectBase;

    public Mesh objectMesh;
    public Vector3[] allVerts;
    public Vector3[] allNormals;
    public Vector2[] allUvs;

    RaycastHit hit;

    public GeneratedMesh leftMesh;
    public GeneratedMesh rightMesh;
    public Mesh leftMesh_Base;
    public Mesh rightMesh_Base;



    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y)), out hit))
            {
                CutMesh(hit.collider.gameObject, hit.point, Vector3.left);
            }
        }
    }

    public void CutMesh(GameObject cutObject, Vector3 contactPoint, Vector3 direction)
    {
        // making a plane in object local space
        Plane bladePlane = new Plane(cutObject.transform.InverseTransformDirection(-direction), cutObject.transform.InverseTransformPoint(contactPoint));
        
        // get the object mesh
        objectMesh = cutObject.GetComponent<MeshFilter>().mesh;

        // extract the vert list
        allVerts = objectMesh.vertices;

        // extract the normal list
        allNormals = objectMesh.normals;

        // extract the UVs
        allUvs = objectMesh.uv;

        // the triangle indices
        int[] triangleIndices = objectMesh.GetTriangles(0);

        leftMesh = new GeneratedMesh();
        rightMesh = new GeneratedMesh();

        int triangleVertA, triangleVertB, triangleVertC;

        for (int i = 0; i < triangleIndices.Length; i += 3)
        {
            // select verticex index and forn triangle
            triangleVertA = triangleIndices[i];
            triangleVertB = triangleIndices[i + 1];
            triangleVertC = triangleIndices[i + 2];

            // create MeshTriangle 
            // get the triangles index list of the specified submesh.
            // make triangles with the selected three points.
            // and their normals and UVs
            MeshTriangle meshTriangle = GenerateMesh(triangleVertA, triangleVertB, triangleVertC);

            // see if ever vertex layes on one side
            // seporate the mesh two sides mesh holders.
            bool vertA_isLeft = bladePlane.GetSide(allVerts[triangleVertA]);
            bool vertB_isLeft = bladePlane.GetSide(allVerts[triangleVertB]);
            bool vertC_isLeft = bladePlane.GetSide(allVerts[triangleVertC]);

            // send the triangel to left
            if (vertA_isLeft && vertB_isLeft && vertC_isLeft) 
            {
                leftMesh.AddTriangle(meshTriangle);
            }

            // send the triangel to right
            else if (!vertA_isLeft && !vertB_isLeft && !vertC_isLeft)
            {
                rightMesh.AddTriangle(meshTriangle);
            }

        }

        // make two game objects and replace the mesh
        GameObject leftObject = Instantiate(objectBase, cutObject.transform.position, cutObject.transform.rotation);
        GameObject rightObject = Instantiate(objectBase, cutObject.transform.position, cutObject.transform.rotation);


        leftMesh_Base = leftMesh.GetMesh();
        rightMesh_Base = rightMesh.GetMesh();

        Debug.Log(leftMesh_Base.vertices.Length);
        Debug.Log(rightMesh_Base.vertices.Length);

        leftObject.GetComponent<MeshFilter>().mesh = leftMesh_Base;
        rightObject.GetComponent<MeshFilter>().mesh = rightMesh_Base;

    }

    public MeshTriangle GenerateMesh(int triVertA, int triVertB, int triVertC) 
    {
        Vector3[] vertes = new Vector3[3] { allVerts[triVertA], allVerts[triVertB], allVerts[triVertC] };
        Vector3[] normals = new Vector3[3] { allNormals[triVertA], allNormals[triVertB], allNormals[triVertC] };
        Vector2[] uvs = new Vector2[3] { allUvs[triVertA], allUvs[triVertB], allUvs[triVertC] };

        return new MeshTriangle(vertes, normals, uvs);
    }

}




[System.Serializable] public class GeneratedMesh
{
    public List<Vector3> vertices = new List<Vector3>();
    public List<Vector3> normals = new List<Vector3>();
    public List<Vector2> uvs = new List<Vector2>();
    public List<int> index = new List<int>();

    public List<Vector3> Vertices { set { vertices = (value); } get { return vertices; } }
    public List<Vector3> Normals { set { normals = (value); } get { return normals; } }
    public List<Vector2> Uvs { set { uvs = (value); } get { return uvs; } }
    public List<int> Index { set { index = (value); } get { return index; } }

    public Mesh GetMesh() 
    {
        Mesh newMesh = new Mesh
        {
            vertices = vertices.ToArray(),
            triangles = index.ToArray(),
            normals = normals.ToArray(),
            uv = uvs.ToArray()
        };
        Debug.Log(newMesh.normals.Length);
        newMesh.RecalculateNormals();
       // newMesh.Optimize();

        return newMesh;
    }


    public void AddTriangle(MeshTriangle meshTriangle) 
    {

        int vertexIndex = index.Count;

        vertices.AddRange(meshTriangle.Vertices);
        normals.AddRange(meshTriangle.Normals);
        uvs.AddRange(meshTriangle.Uvs);

        for (int i = 0; i < 3; i++)
        {
            Index.Add(vertexIndex + i);
        }

    }

}


[System.Serializable] public class MeshTriangle
{
    public List<Vector3> vertices = new List<Vector3>();
    public List<Vector3> normals = new List<Vector3>();
    public List<Vector2> uvs = new List<Vector2>();
    public List<int> index = new List<int>();

    public List<Vector3> Vertices { set { vertices = (value); } get { return vertices; } }
    public List<Vector3> Normals { set { normals = (value); } get { return normals; } }
    public List<Vector2> Uvs { set { uvs = (value); } get { return uvs; } }
    public List<int> Index { set { index = (value); } get { return index; } }

    public MeshTriangle(Vector3[] _vertices, Vector3[] _normals, Vector2[] _uvs) 
    {
        vertices.AddRange(_vertices);
        normals.AddRange(_normals);
        uvs.AddRange(_uvs);
    }

    public void Clear() {
        vertices.Clear();
        normals.Clear();
        uvs.Clear();
    }


}