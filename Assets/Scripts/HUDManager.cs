using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;
    // Start is called before the first frame update
    public TextMeshProUGUI StateText;
    public TextMeshProUGUI HeathText;
    public TextMeshProUGUI RollText;
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

    public void UpdateHeat(float value){
        HeathText.text = "Heat: " + value.ToString("0.00");
    }

    public void UpdateCanRoll(bool canRoll){
        RollText.text = "Can Roll: " + canRoll.ToString();
    }
}
