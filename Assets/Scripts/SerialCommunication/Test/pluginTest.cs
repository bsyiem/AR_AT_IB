using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pluginTest : MonoBehaviour
{
    void Start()
    {

        AndroidJavaClass pluginTest = new AndroidJavaClass("com.bsyiem.serialcommunicationplugin.Test");
        GetComponent<TextMesh>().text = pluginTest.CallStatic<string>("getMessage");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
