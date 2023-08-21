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

    public float CurrentHeat = 0f;
    public float HeathDissapationRate = 1f;
    public float MaxHeat = 100f;
    public float DiveHeatRate = 2f;
    public float RollHeat = 10f;

    public bool CanRoll = true;
    public float RollCooldownMax = 1f;
    public float RollCooldownTimer = 0f;
    public float RollAdjustAmount = 3f;
    public float CurrentRollAdjust = 0f;

    public bool RollLeftReset = true;
    public bool RollRightReset = true;
    public GameObject Avatar;
    public SpriteRenderer PlayerSprite;

    void Start()
    {
        mainCam = Camera.main;
        playerCtrls = GameManager.instance.Ctrls.Player;
        Restart();
    }

    public void Restart(){
        playerCtrls.Enable();
        CurrentHeat = 0f;
        MaxHeat = 100f;
        HeathDissapationRate = 1f;
        DiveHeatRate = 2f;
        RollHeat = 10f;
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
        bool inputRollRight = playerCtrls.RollRight.ReadValue<float>() > 0.1f;
        bool inputRollLeft = playerCtrls.RollLeft.ReadValue<float>() > 0.1f;


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


        
        if(CurrentFallState == FallState.Dive) {
            CurrentHeat += DiveHeatRate * Time.deltaTime;            
        }
        else{
            CurrentHeat -= HeathDissapationRate * Time.deltaTime;
        }


        //Debug.Log($"{inputRollLeft} | {inputRollRight}");
        if(CanRoll){          
            if(inputRollLeft && RollLeftReset){
                Debug.Log("Roll Left");
                RollLeftReset = false;
                LeanTween.rotateY(gameObject, 30f, 0.3f);
                CurrentRollAdjust = -RollAdjustAmount;
                RollCooldownTimer = RollCooldownMax;
            }
            else if(inputRollRight && RollRightReset){
                Debug.Log("Roll Right");
                LeanTween.rotateY(gameObject, -30f, 0.3f);
                RollRightReset = false;
                CurrentRollAdjust = RollAdjustAmount;
                RollCooldownTimer = RollCooldownMax;
            }
        }

        this.transform.position = Vector3.SmoothDamp(transform.position, worldPos + new Vector3(CurrentRollAdjust, 0f, 0f), ref velocity, 1f / CurrentSpeed);

        RollRightReset = playerCtrls.RollRight.ReadValue<float>() <= 0.1f;
        RollLeftReset = playerCtrls.RollLeft.ReadValue<float>() <= 0.1f;

        if(RollCooldownTimer > 0){
            if(CanRoll){
                CanRoll = false;
                PlayerSprite.color = Color.gray;
                CurrentHeat += RollHeat;
                HUDManager.instance.UpdateCanRoll(CanRoll);
            }

            RollCooldownTimer -= Time.deltaTime;
        }
        else if(RollCooldownTimer < 0) {
            RollCooldownTimer = 0f;

            if(!CanRoll){
                ReleaseRoll();
            }
        }

        if(RollLeftReset && RollRightReset){
            AbandonRoll();
        }

        if(CurrentHeat < 0){
            CurrentHeat = 0;
        }

        HUDManager.instance.UpdateHeat(CurrentHeat);
        if(CurrentHeat >= MaxHeat){
            Debug.LogWarning("You Lose");
        }
    }

    private void AbandonRoll(){
        LeanTween.rotateY(gameObject, 0f, 0.3f);
        PlayerSprite.color = Color.white;
        CurrentRollAdjust = 0f;
    }

    private void ReleaseRoll(){
        CanRoll = true;
        AbandonRoll();
        HUDManager.instance.UpdateCanRoll(CanRoll);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning(other.name);
    }
}
