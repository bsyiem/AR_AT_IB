using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//used to hold application settings i.e., which scene to load next 

//should turn this to a singleton
public class MTurkSettings : MonoBehaviour
{
    public static string SCREEN_CODE = "UOM-AR-AT";

    //used to keep track of current scene so that instruction controller provides the current instructions
    public enum StudyScenes
    {
        OPN, // opening scene
        TUTOTIAL, 
        INS_1, //instructions for first task i.e., pnn or vnn
        INS_2, //instructions for second task  i.e., pyn or vyn
        INS_3,
        INS_4,
        INS_5,
        INS_6,
        EXP //  Current scene is an experiment.
    };

    public StudyScenes currentScene = StudyScenes.OPN;
    public int currentCondition = 0; // 0 represent the tutorial scene
    public int pId = -1;
    public string startingEventType;

    private static MTurkSettings _instance;

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
