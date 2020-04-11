using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorStorage : MonoBehaviour
{
  public Mesh mesh;
  public Material material;

    public static EditorStorage instance;

    public void Awake()
    {
        instance = this;
    }

    public void Update()
    {
       Graphics.DrawMesh(mesh, transform.position, transform.rotation, material, 0);
    }

}
