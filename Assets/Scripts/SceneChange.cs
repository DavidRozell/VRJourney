using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{

    public void ChangeToNormalScene()
    {
        SceneManager.LoadScene("Final_PicsAndVideos");
    }

    public void ChangeToEpicScene()
    {
        SceneManager.LoadScene("Final_Epic");
    }

}
