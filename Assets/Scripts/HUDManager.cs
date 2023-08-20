using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;
    // Start is called before the first frame update
    public TextMeshProUGUI StateText;

    void Awake()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateStateText(string state){
        StateText.text = "State: " + state;
    }
}
