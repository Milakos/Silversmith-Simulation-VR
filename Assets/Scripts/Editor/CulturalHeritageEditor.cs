using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CulturalHeritageEditor : EditorWindow
{

    GameObject prefab;
    GameObject socket;
    string animationName;

    [MenuItem("Window/VR Cultural Heritage/Grabbable Object with Socket")]

    public static void ShowWindow() 
    {
        GetWindow<CulturalHeritageEditor>("VR Museums");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create Interactable Prefab Properties", EditorStyles.boldLabel);
        EditorGUILayout.Space(10, false);

        prefab =  (GameObject)EditorGUILayout.ObjectField(prefab, typeof(GameObject), false);
        EditorGUILayout.Space(10, false);
        
        socket = (GameObject)EditorGUILayout.ObjectField(socket, typeof(GameObject), false);
        EditorGUILayout.Space(10, false);

        animationName = GUILayout.TextField(animationName);
        EditorGUILayout.Space(10, false);

        var btn = GUILayout.Button("Spawn");

/*        if (btn) 
        {
            if (prefab != null)
                Instantiate(prefab, GameObject.Find("-----[GRAB INTERACTABLES]------").transform);
            if (socket != null)
                Instantiate(socket, GameObject.Find("-----[SOCKET INTERACTORS]-----").transform);
            if (prefab != null) 
            {
                prefab.GetComponent<InteractableManager>().ItemsSocket = socket;               
            }
            if (prefab != null) 
            {
                prefab.GetComponent<InteractableManager>().AnimationNameHash = animationName;
            }
            


            Close();
        }*/
    }
}
