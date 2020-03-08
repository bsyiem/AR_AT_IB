using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControllerTestEmission : MonoBehaviour
{

    public GameObject ledPrefab;
    public float led_z_distance = 15f;


    // Start is called before the first frame update
    void Start()
    {
        GameObject led =  Instantiate(ledPrefab, Vector3.zero, Quaternion.identity);
        led.GetComponent<LedBehaviour>().setRelativePosition(new Vector3(0.5f, 0.5f, led_z_distance));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
