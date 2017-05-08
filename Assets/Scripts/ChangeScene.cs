/*
This script holds a method for changing the screen/game state.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void changeScene(string sceneName)
    {
        //Application.LoadLevel (sceneName);
        SceneManager.LoadScene(sceneName);
    }
}