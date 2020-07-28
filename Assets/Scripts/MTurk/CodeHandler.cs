using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeHandler : MonoBehaviour
{
    public UnityEngine.UI.Text closingText;

    private MTurkSettings settings;
    private PHPCommunicationManager commManager;

    private void Awake()
    {
        this.commManager = GameObject.Find("CommunicationManager").GetComponent<PHPCommunicationManager>();
        this.settings = GameObject.Find("Settings").GetComponent<MTurkSettings>();
    }

    // Start is called before the first frame update
    void Start()
    {
        int randomCode = (int)Mathf.Round(Random.Range(100,999));

        string generatedCode = MTurkSettings.SCREEN_CODE + "_" + this.settings.pId + "_" + randomCode.ToString();

        this.closingText.text += "\nPlease note down this code: " + generatedCode;

        this.commManager.sendCode(this.settings.pId, generatedCode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
