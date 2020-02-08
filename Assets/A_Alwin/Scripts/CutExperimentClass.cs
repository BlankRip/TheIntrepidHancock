using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutExperimentClass : MonoBehaviour
{
    public LayerMask breakableLayer;
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

    public Vector3 centrePointSum;


    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y)), out hit, breakableLayer))
            {
                Reset();
                CutMesh(hit.collider.gameObject, hit.point, Vector3.left);
            }
        }
    }

    public void Reset() 
    {
        centrePointSum = Vector3.zero;
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

        // edges used to finally cap the end of the cut mesh
        List<CutEdge> allEdges = new List<CutEdge>();

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

            // cut the triangle
            else 
            {
                CutTriangle(bladePlane, meshTriangle, vertA_isLeft, vertB_isLeft, vertC_isLeft, leftMesh, rightMesh, allEdges);
            }

        }

        // cap the edge
        CapTheEdge(bladePlane, leftMesh, rightMesh, allEdges);

        // make two game objects and replace the mesh
        GameObject leftObject = Instantiate(cutObject, cutObject.transform.position, cutObject.transform.rotation);
        GameObject rightObject = Instantiate(cutObject, cutObject.transform.position, cutObject.transform.rotation);

        leftMesh_Base = leftMesh.GetMesh();
        rightMesh_Base = rightMesh.GetMesh();

        leftObject.GetComponent<MeshFilter>().mesh = leftMesh_Base;
        leftObject.GetComponent<MeshCollider>().sharedMesh = leftMesh_Base;

        rightObject.GetComponent<MeshFilter>().mesh = rightMesh_Base;
        rightObject.GetComponent<MeshCollider>().sharedMesh = rightMesh_Base;

        Destroy(cutObject);

    }

    public void CapTheEdge(Plane blade, GeneratedMesh leftMesh, GeneratedMesh rightMesh, List<CutEdge> edgeList) 
    {
        centrePointSum /= edgeList.Count * 2;
        // go through the edges and make triangles
        for (int i = 0; i < edgeList.Count; i++)
        {
            // triangle making cap
            Vector3[] triangeVerts = new Vector3[3] { centrePointSum, edgeList[i].Edge[0], edgeList[i].Edge[1]};

            // create and add left side triangle
            Vector3[] triangleLeftNormal = new Vector3[3] { -blade.normal, -blade.normal, -blade.normal };
            MeshTriangle triangleLeftSide = new MeshTriangle(triangeVerts, triangleLeftNormal, new Vector2[3]);
            CorrectTriangle(triangleLeftSide, triangeVerts, -blade.normal);
            leftMesh.AddTriangle(triangleLeftSide);
            // create and add left side triangle
            Vector3[] triangleRightNormal = new Vector3[3] { blade.normal, blade.normal, blade.normal };
            MeshTriangle triangleRightSide = new MeshTriangle(triangeVerts, triangleRightNormal, new Vector2[3]);
            CorrectTriangle(triangleRightSide, triangeVerts, blade.normal);
            rightMesh.AddTriangle(triangleRightSide);
        }
    }

    public MeshTriangle GenerateMesh(int triVertA, int triVertB, int triVertC) 
    {
        Vector3[] vertes = new Vector3[3] { allVerts[triVertA], allVerts[triVertB], allVerts[triVertC] };
        Vector3[] normals = new Vector3[3] { allNormals[triVertA], allNormals[triVertB], allNormals[triVertC] };
        Vector2[] uvs = new Vector2[3] { allUvs[triVertA], allUvs[triVertB], allUvs[triVertC] };

        return new MeshTriangle(vertes, normals, uvs);
    }

    public void CutTriangle(Plane blade, MeshTriangle _triangle, bool _triangleVertA, bool _triangleVertB, bool _triangleVertC, GeneratedMesh leftMesh, GeneratedMesh rightMesh, List<CutEdge> edgeList) 
    {
        // holding all the vertex side info into a list
        List<bool> pointsAtLeft = new List<bool>(3);
        pointsAtLeft.Add(_triangleVertA);
        pointsAtLeft.Add(_triangleVertB);
        pointsAtLeft.Add(_triangleVertC);

        // making two data holders
        MeshTriangle leftTriangle = new MeshTriangle(new Vector3[2], new Vector3[2], new Vector2[2]);
        MeshTriangle rightTriangle = new MeshTriangle(new Vector3[2], new Vector3[2], new Vector2[2]);

        bool wasAtLeft = false, wasAtRight = false;

        // goes through the sides of the vert side list to populate the data holder
        for (int i = 0; i < 3; i++)
        {
            // point at left side of plane
            if (pointsAtLeft[i])
            {
                // if this is the first left poit
                if (!wasAtLeft)
                {
                    wasAtLeft = true;
                    // make the first left triangle vertex to
                    // place holder triangle left vertex 1
                    leftTriangle.Vertices[0] = _triangle.Vertices[i];
                    leftTriangle.Vertices[1] = leftTriangle.Vertices[0];
                    // make the first left triangle UV to
                    // place holder triangle left UV 1
                    leftTriangle.Uvs[0] = _triangle.Uvs[i];
                    leftTriangle.Uvs[1] = leftTriangle.Uvs[0];
                    // make the first left triangle Normal to
                    // place holder triangle left Normal 1
                    leftTriangle.Normals[0] = _triangle.Normals[i];
                    leftTriangle.Normals[1] = leftTriangle.Normals[0];
                }

                // if this is the second left vertex point
                else
                {
                    // update all point 1 data of new triangle to second
                    // left side point
                    leftTriangle.Vertices[1] = _triangle.Vertices[i];
                    leftTriangle.Uvs[1] = _triangle.Uvs[i];
                    leftTriangle.Normals[1] = _triangle.Normals[i];
                }
            }
            // point at the right side 
            else 
            {
                // if this is the first left poit
                if (!wasAtRight)
                {
                    wasAtRight = true;
                    // make the first right triangle vertex to
                    // place holder triangle right vertex 1
                    rightTriangle.Vertices[0] = _triangle.Vertices[i];
                    rightTriangle.Vertices[1] = rightTriangle.Vertices[0];
                    // make the first right triangle UV to
                    // place holder triangle right UV 1
                    rightTriangle.Uvs[0] = _triangle.Uvs[i];
                    rightTriangle.Uvs[1] = rightTriangle.Uvs[0];
                    // make the first right triangle Normal to
                    // place holder triangle right Normal 1
                    rightTriangle.Normals[0] = _triangle.Normals[i];
                    rightTriangle.Normals[1] = rightTriangle.Normals[0];
                }

                // if this is the second right vertex point
                else
                {
                    // update all point 1 data of new triangle to second
                    // right side point
                    rightTriangle.Vertices[1] = _triangle.Vertices[i];
                    rightTriangle.Uvs[1] = _triangle.Uvs[i];
                    rightTriangle.Normals[1] = _triangle.Normals[i];
                }
            }
        }


        // find and add the slice vertex

        float lineRelativeMagnitude = 0;
        float distance = 0;
        Vector3 dirVector;


        // cast from left triangle vert 1 to right triangle vert 1
        dirVector = rightTriangle.Vertices[0] - leftTriangle.Vertices[0];
        blade.Raycast(new Ray(leftTriangle.Vertices[0], dirVector), out distance);
        lineRelativeMagnitude = distance / dirVector.magnitude;

        // add the new vertex
        Vector3 leftVert = Vector3.Lerp(leftTriangle.Vertices[0], rightTriangle.Vertices[0], lineRelativeMagnitude);
        leftTriangle.Vertices.Add(leftVert);
        // add the new normal
        Vector3 leftNormal = Vector3.Lerp(leftTriangle.Normals[0], rightTriangle.Normals[0], lineRelativeMagnitude);
        leftTriangle.Normals.Add(leftNormal);
        // add the new UV
        Vector2 leftUV = Vector3.Lerp(leftTriangle.Uvs[0], rightTriangle.Uvs[0], lineRelativeMagnitude);
        leftTriangle.Uvs.Add(leftUV);


        // cast from right triangle vert 2 to left triangle vert 2
        dirVector = leftTriangle.Vertices[1] - rightTriangle.Vertices[1];
        blade.Raycast(new Ray(rightTriangle.Vertices[1], dirVector), out distance);
        lineRelativeMagnitude = distance / dirVector.magnitude;

        // add the new vertex
        Vector3 rightVert = Vector3.Lerp(rightTriangle.Vertices[1], leftTriangle.Vertices[1], lineRelativeMagnitude);
        rightTriangle.Vertices.Add(rightVert);
        // add the new normal
        Vector3 rightNormal = Vector3.Lerp(rightTriangle.Normals[1], leftTriangle.Normals[1], lineRelativeMagnitude);
        rightTriangle.Normals.Add(rightNormal);
        // add the new UV
        Vector2 rightUV = Vector3.Lerp(rightTriangle.Uvs[1], leftTriangle.Uvs[1], lineRelativeMagnitude);
        rightTriangle.Uvs.Add(rightUV);

        // add the new edge
        // create edge
        CutEdge chopEdge = new CutEdge();
        chopEdge.Edge = new Vector3[2] { leftVert, rightVert };
        edgeList.Add(chopEdge);

        // recording the centre vector
        centrePointSum += leftVert + rightVert;

        // add the new triangle

        // check for the left side triangles
        MeshTriangle currentTriangel;
        Vector3[] updatedVerts = new Vector3[] { leftTriangle.Vertices[0], leftVert, rightVert };
        Vector3[] updatedNormals = new Vector3[] { leftTriangle.Normals[0], leftNormal, rightNormal };
        Vector2[] updatedUVs = new Vector2[] { leftTriangle.Uvs[0], leftUV, rightUV };

        currentTriangel = new MeshTriangle(updatedVerts, updatedNormals, updatedUVs);

        // check if the triangle has same verts 
        // creates a single triangle closhure
        if (updatedVerts[0] != updatedVerts[1] && updatedVerts[0] != updatedVerts[2]) 
        {
            // chech the normal
            // flip the triangle
            CorrectTriangle(currentTriangel, updatedVerts, updatedNormals[0]);
            leftMesh.AddTriangle(currentTriangel);
        }

        updatedVerts = new Vector3[] { leftTriangle.Vertices[0], leftTriangle.Vertices[1], rightVert };
        updatedNormals = new Vector3[] { leftTriangle.Normals[0], leftTriangle.Normals[1], rightNormal };
        updatedUVs = new Vector2[] { leftTriangle.Uvs[0], leftTriangle.Uvs[1], rightUV };

        currentTriangel = new MeshTriangle(updatedVerts, updatedNormals, updatedUVs);

        // adds the second triangle to reate a rombus
        if (updatedVerts[0] != updatedVerts[1] && updatedVerts[0] != updatedVerts[2])
        {
            // chech the normal
            // flip the triangle
            CorrectTriangle(currentTriangel, updatedVerts, updatedNormals[0]);
            leftMesh.AddTriangle(currentTriangel);
        }


        // check for the right side triangles
        
        updatedVerts = new Vector3[] { rightTriangle.Vertices[0], leftVert, rightVert };
        updatedNormals = new Vector3[] { rightTriangle.Normals[0], leftNormal, rightNormal };
        updatedUVs = new Vector2[] { rightTriangle.Uvs[0], leftUV, rightUV };

        currentTriangel = new MeshTriangle(updatedVerts, updatedNormals, updatedUVs);

        // check if the triangle has same verts 
        // creates a single triangle closhure
        if (updatedVerts[0] != updatedVerts[1] && updatedVerts[0] != updatedVerts[2])
        {
            // chech the normal
            // flip the triangle
            CorrectTriangle(currentTriangel, updatedVerts, updatedNormals[0]);
            rightMesh.AddTriangle(currentTriangel);
        }

        updatedVerts = new Vector3[] { rightTriangle.Vertices[0], rightTriangle.Vertices[1], rightVert };
        updatedNormals = new Vector3[] { rightTriangle.Normals[0], rightTriangle.Normals[1], rightNormal };
        updatedUVs = new Vector2[] { rightTriangle.Uvs[0], rightTriangle.Uvs[1], rightUV };

        currentTriangel = new MeshTriangle(updatedVerts, updatedNormals, updatedUVs);

        // adds the second triangle to reate a rombus
        if (updatedVerts[0] != updatedVerts[1] && updatedVerts[0] != updatedVerts[2])
        {
            // chech the normal
            // flip the triangle
            CorrectTriangle(currentTriangel, updatedVerts, updatedNormals[0]);
            rightMesh.AddTriangle(currentTriangel);
        }

    }

    void CorrectTriangle(MeshTriangle triangle, Vector3[] newVertList, Vector3 matchNormal) 
    {
        Vector3 triangleNormal = Vector3.Cross(newVertList[1] - newVertList[0], newVertList[2] - newVertList[0]);
        if (Vector3.Dot(triangleNormal, matchNormal) < 0) 
        {
            triangle.Flip();
        }
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
   //     newMesh.RecalculateNormals();
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

public class CutEdge
{
    Vector3[] edge;

    public Vector3[] Edge { get {return edge;} set {edge = value;}}
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

    public void Flip ()
    {
        Vector3 vertB = vertices[1];
        vertices[1] = vertices[2];
        vertices[2] = vertB;
        Vector3 normalB = normals[1];
        normals[1] = normals[2];
        normals[2] = normalB;
        Vector3 uvB = uvs[1];
        uvs[1] = uvs[2];
        uvs[2] = uvB;
    }
}