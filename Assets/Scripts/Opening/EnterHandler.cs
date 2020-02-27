using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum MScenes
{
    physicalEventAR,
    virtualEventAR,
    physicalEventNoAR,
    virtualEventNoAR,
    testScene
}

public class EnterHandler : MonoBehaviour
{
    public FileSetting fileSetting;
    public InputField textObj;
    public MScenes scene;
    

    public void SetSharedPreferenceFileName()
    {
        fileSetting.fileName = textObj.text;

        switch (scene)
        {
            case MScenes.physicalEventAR:
                SceneManager.LoadScene("physicalEventAR");
                break;
            case MScenes.virtualEventAR:
                SceneManager.LoadScene("virtualEventAR");
                break;
            case MScenes.physicalEventNoAR:
                SceneManager.LoadScene("physicalEventNoAR");
                break;
            case MScenes.virtualEventNoAR:
                SceneManager.LoadScene("virtualEventNoAR");
                break;
            case MScenes.testScene:
                SceneManager.LoadScene("testScene");
                break;
        }
    }
}
