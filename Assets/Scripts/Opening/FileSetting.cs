using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileSetting : MonoBehaviour
{

    public string fileName { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
