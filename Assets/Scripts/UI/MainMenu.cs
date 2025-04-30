#if UNITY_EDITOR
    using UnityEditor;
#endif

using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void OnEnable() 
    {
        FindObjectOfType<ButtonActionsController>().yButton += QuitGame;    
    }

/*    private void OnDisable() 
    {
        
        FindObjectOfType<ButtonActionsController>().yButton -= QuitGame; 
    }*/
    public void QuitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
        
    }
}
