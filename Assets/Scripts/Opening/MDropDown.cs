using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MScenes
{
    physicalEventAR,
    virtualEventAR,
    physicalEventNoAR,
    virtualEventNoAR,
    testScene,
    testEmission
}

public class MDropDown : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        foreach(MScenes value in Enum.GetValues(typeof(MScenes)))
        {
            this.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(value.ToString()));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
