using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum FallState { Normal, Dive, Glide }


    Camera mainCam;
    Controls.PlayerActions playerCtrls;

    public FallState CurrentFallState;

    public float GlideSpeed = 1.2f;
    public float FallSpeed = 0.9f;
    public float DiveSpeed = 0.5f;

    public Vector3 FallSize = Vector3.one;
    public Vector3 DiveSize = Vector3.one;
    public Vector3 GlideSize = Vector3.one;

    float CurrentSpeed;
    Vector3 velocity = Vector3.zero;

    public GameObject Avatar;

    void Start()
    {
        mainCam = Camera.main;
        playerCtrls = GameManager.instance.Ctrls.Player;
        Restart();
    }

    public void Restart(){
        playerCtrls.Enable();
        Fall();
    }

    public void Die(){
        playerCtrls.Disable();
    }

    public void Fall(){
        CurrentFallState = FallState.Normal;
        CurrentSpeed = FallSpeed;
        GameManager.instance.UpdateSpeed(FallSpeed);
        HUDManager.instance.UpdateStateText("Fall");
        LeanTween.scale(Avatar, FallSize, 0.3f).setEaseInOutCirc();
    }

    public void Dive(){
        CurrentFallState = FallState.Dive;
        CurrentSpeed = DiveSpeed;
        GameManager.instance.UpdateSpeed(DiveSpeed);
        HUDManager.instance.UpdateStateText("Dive");
        LeanTween.scale(Avatar, DiveSize, 0.3f).setEaseInOutCirc();
    }

    public void Glide(){
        CurrentFallState = FallState.Glide;
        CurrentSpeed = GlideSpeed;
        GameManager.instance.UpdateSpeed(GlideSpeed);
        HUDManager.instance.UpdateStateText("Glide");
        LeanTween.scale(Avatar, GlideSize, 0.3f).setEaseInOutCirc();
    }

    void Update()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition = new Vector3(Mathf.Clamp(mousePosition.x, 0f, Screen.width),
                                    Mathf.Clamp(mousePosition.y, 0, Screen.height),
                                    mousePosition.z);
        

        mousePosition.z = 10f;
        Vector3 worldPos = mainCam.ScreenToWorldPoint(mousePosition);
        worldPos.z = 1f; //Change this to force screen border

        bool inputDive = playerCtrls.Dive.ReadValue<float>() > 0.1f;
        bool inputGlide = playerCtrls.Glide.ReadValue<float>() > 0.1f;

        //Debug.Log(inputDive + " | " + inputGlide);
        if(inputDive){
            if(CurrentFallState != FallState.Dive) Dive();
        }
        else if(inputGlide){
            if(CurrentFallState != FallState.Glide) Glide();
        }
        else if(CurrentFallState != FallState.Normal) {
            Fall();
        }
        this.transform.position = Vector3.SmoothDamp(transform.position, worldPos, ref velocity, 1f / CurrentSpeed);
        //this.transform.position = worldPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning(other.name);
    }
}
