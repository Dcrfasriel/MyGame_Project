using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnSceneLoad : MonoBehaviour
{
    private void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainGame"));
    }
}
