using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsbSerialManager : MonoBehaviour
{

    AndroidJavaClass UsbSerialManagerClass;
    AndroidJavaObject instance { get { return UsbSerialManagerClass.GetStatic<AndroidJavaObject>("INSTANCE"); } }

    // Start is called before the first frame update
    void Start()
    {
        this.UsbSerialManagerClass = new AndroidJavaClass("com.bsyiem.serialcommunicationplugin.UsbSerialManager");
        this.UsbSerialManagerClass.CallStatic("instantiate", this.gameObject.name);
        //this.instance.Call("showText", "is this working?");
        this.instance.Call("createUsbSerialDevice");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
