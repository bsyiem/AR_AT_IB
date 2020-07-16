using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SubmitController : MonoBehaviour
{
    public UnityEngine.UI.InputField inputCount;


    private PHPCommunicationManager commManager;
    private MTurkSettings settings;

    // Start is called before the first frame update
    void Start()
    {
        this.commManager = GameObject.Find("CommunicationManager").GetComponent<PHPCommunicationManager>();
        this.settings = GameObject.Find("Settings").GetComponent<MTurkSettings>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubmitCount()
    {
        int count;
        if(!int.TryParse(inputCount.text, out count))
        {
            inputCount.text = "";
            inputCount.placeholder.GetComponent<UnityEngine.UI.Text>().text = "Please enter a whole number";
        }
        else
        {
            this.commManager.sendPassCount(count);

            if (this.settings.currentCondition > 6)
            {
                SceneManager.LoadScene("MTurkEnd");
            }
            SceneManager.LoadScene("MTurk Instructions");
        }
    }
}
