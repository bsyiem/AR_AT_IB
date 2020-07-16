using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PHPCommunicationManager : MonoBehaviour
{
    private static PHPCommunicationManager _instance;

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //read previous partocipant number
    //this is used to alternate starting event type
    public string getLastParticipantNumber()
    {
        string pId = "";

        //read pId;

        return pId;
    }

    //sends the current generated participant number along with the current conditions
    //Example: participant number = p1, currentCondition = pyy
    // different conditions =  eventType_AR_Task
    // so vyn = virtual event, yes - AR elements, no Task.
    //usually getLastParticipantNumber() + 1
    //this will determine the file name.
    public void sendFileName(string pnumber, string condition)
    {

    }


    //
    public void sendReactionTimeEvent(string reactionType, float reactionTime, int ledNumber)
    {

    }

    public void sendPassCount(int passCount)
    {

    }
}
