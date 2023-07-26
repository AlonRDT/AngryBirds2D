using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILogic : MonoBehaviour
{
    public void ReloadLevel()
    {
        SceneManager.LoadScene("Level1");
    }
}
