using UnityEngine;
using UnityEngine.InputSystem;

public class Exit : MonoBehaviour
{
    private const Key EXIT_KEY = Key.Escape;
    void Update()
    {
        if(Keyboard.current[EXIT_KEY].wasPressedThisFrame)
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            # elif UNITY_STANDALONE
            Application.Quit();
            #endif
        }
    }
}
