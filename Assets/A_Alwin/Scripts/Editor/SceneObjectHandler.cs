using UnityEngine;
using UnityEditor;

public class SceneObjectHandler : EditorWindow
{

    public string objName = "Name";
    public Vector3 pos;
    GameObject closestObjectToCursor;
    Vector2 mousePos;
    Color selectedColorA, selectedColorB;
bool runEditor = false, canFlash;
    public GUIStyle planeStyle;

    [MenuItem("Window/Custom")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<SceneObjectHandler>("Edit Handler");
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Flash Color", EditorStyles.boldLabel);
        selectedColorA = EditorGUILayout.ColorField(selectedColorA,GUILayout.MaxWidth (100));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Can Flash", EditorStyles.boldLabel);
        canFlash = EditorGUILayout.Toggle(canFlash);
        GUILayout.EndHorizontal();
/*
        if(closestObjectToCursor)objName = EditorGUILayout.TextField("Object Name", objName);
        else EditorGUILayout.TextField("Object Name", "Nothing");
        */
    }

     void OnFocus() {
        runEditor = false;
        mousePos = new Vector2();
        planeStyle = new GUIStyle(EditorStyles.label);
        planeStyle.normal.textColor = Color.white;

     // Remove delegate listener if it has previously
     // been assigned.
     SceneView.duringSceneGui -= this.OnSceneGUI;

     // Add (or re-add) the delegate.
     SceneView.duringSceneGui += this.OnSceneGUI;
 }

    void OnDestroy() {
     // When the window is destroyed, remove the delegate
     // so that it will no longer do any drawing.
     SceneView.duringSceneGui -= this.OnSceneGUI;
    }

    void OnSceneGUI(SceneView sceneView)
    {
        

        Event e = Event.current;

         switch (e.type) {
             case EventType.KeyDown:
            
             if(e.keyCode == KeyCode.LeftControl)
             {
                  runEditor = true;
             }
                 break;

            case EventType.KeyUp:
            
             if(e.keyCode == KeyCode.LeftControl)
             {
                  runEditor = false;
             }
                 break;
         }
if(runEditor){
        if(e.type == EventType.MouseMove){
            closestObjectToCursor = HandleUtility.PickGameObject(e.mousePosition, true);
            if(closestObjectToCursor != null){
                objName = closestObjectToCursor.name;
                pos = closestObjectToCursor.transform.position;
                mousePos.x = e.mousePosition.x;
                mousePos.y = e.mousePosition.y;
            }
          
        }

        if(e.type == EventType.Repaint && closestObjectToCursor != null)
        {
            Transform[] allGameObjects = closestObjectToCursor.GetComponentsInChildren<Transform>();
           
           
           
            Camera cam = sceneView.camera;
            Shader shader = Shader.Find("Unlit/EditorHeilight");
            Material mat = new Material(shader);
            mat.SetVector("_FlashColor", (Vector4)selectedColorA);
            float flickValue = canFlash? 1 : 0;
            mat.SetFloat("_Flicker", flickValue);
            foreach (Transform item in allGameObjects)
            {
                MeshFilter meshFilter = item.GetComponent<MeshFilter>();
               
                if(meshFilter != null){
                    Mesh mesh =  meshFilter.sharedMesh;
                    Matrix4x4 matrix = item.transform.localToWorldMatrix;
                    Graphics.DrawMesh(mesh, matrix, mat, 0, cam);
                }
            }

        }
}
            if(closestObjectToCursor == null){
                mousePos.x = 0;
                mousePos.y = 0;
            }
            
                Handles.BeginGUI();
            
             //   GUILayout.BeginArea(new Rect(mousePos.x - 50, mousePos.y + 20, 100, 60));
                GUILayout.BeginArea(new Rect(0, 0, 100, 60));
           
                var rect = EditorGUILayout.BeginVertical();
                GUI.color = new Color(0,0,0,0.8f);
                GUI.Box(rect, GUIContent.none);
                
                GUI.color = new Color(1,1,0,1);
              
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(objName, planeStyle);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();

                GUILayout.EndArea();
               
               // Handles.
                Handles.EndGUI();

        }

    void OnDrawMesh(Camera camera, Matrix4x4 matrix)
    {
     //  Graphics.DrawMesh(closestObjectToCursor.GetComponent<MeshFilter>().mesh, matrix, new Material("material"), closestObjectToCursor.layer, camera);
         Graphics.DrawMesh(closestObjectToCursor.GetComponent<MeshFilter>().mesh, Vector3.zero, Quaternion.identity, closestObjectToCursor.GetComponent<MeshRenderer>().material, 1, camera);
    }

}
