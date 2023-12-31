using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float Speed;

    public Controls Ctrls;
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
        Ctrls = new Controls();
        Ctrls.Enable();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void UpdateSpeed(float speed){
        Speed = speed;
    }

    void OnDisable(){
        Ctrls.Disable();
    }
}
