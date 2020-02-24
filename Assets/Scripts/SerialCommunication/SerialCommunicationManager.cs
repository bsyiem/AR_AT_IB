using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerialCommunicationManager : MonoBehaviour
{
    AndroidJavaClass serialCommunicationManagerClass;
    AndroidJavaObject instance { get { return serialCommunicationManagerClass.GetStatic<AndroidJavaObject>("INSTANCE"); } }

    // Start is called before the first frame update
    void Start()
    {
        this.serialCommunicationManagerClass = new AndroidJavaClass("com.bsyiem.serialcommunicationplugin.SerialCommunication");
        this.serialCommunicationManagerClass.CallStatic("instantiate", this.gameObject.name);
        //this.instance.Call("showText", "is this working?");
        this.instance.Call("createPhysicaloid",9600);
        this.instance.Call("openConnection");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
