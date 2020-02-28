using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class EnterHandler : MonoBehaviour
{
    public FileSetting fileSetting;
    public InputField textObj;
    public Dropdown dropdown;


    string scene;

    public void SetSharedPreferenceFileName()
    {
        fileSetting.fileName = textObj.text;

        scene = dropdown.options[dropdown.value].text;
        switch (scene)
        {
            case "physicalEventAR":
                SceneManager.LoadScene("physicalEventAR");
                break;
            case "virtualEventAR":
                SceneManager.LoadScene("virtualEventAR");
                break;
            case "physicalEventNoAR":
                SceneManager.LoadScene("physicalEventNoAR");
                break;
            case "virtualEventNoAR":
                SceneManager.LoadScene("virtualEventNoAR");
                break;
            case "testScene":
                SceneManager.LoadScene("testScene");
                break;
        }
    }
}
