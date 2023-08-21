using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObstacle : MonoBehaviour
{
    public bool ShouldInitScale = true;
    public float SpawnScaleUpSpeed = 0.3f;
    public Vector2 ScaleMinMax = Vector2.one;
    Vector3 origScale;

    public Vector3 MoveDirection;
    public float DestroyTime = 20f;
    
    public Vector2 RotationSpeedMinMax;
    private Vector3 rotationSpeed;    
    public Vector2 InitRotationMinMax = Vector2.one;

    public List<Sprite> SpriteList;
    public SpriteRenderer SpriteRend;

    public bool ShouldUseAlpha = false;
    public float AlphaFadeDistance = 100f;
    public float AlphaFadeOffsetPosition = 20f;
    // Start is called before the first frame update
    void Start()
    {
        if(SpriteList.Count > 0){
            SpriteRend.sprite = SpriteList[Random.Range(0, SpriteList.Count)];
        }        

        origScale = this.transform.localScale * Random.Range(ScaleMinMax.x, ScaleMinMax.y);
        if(ShouldInitScale){            
            this.transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject, origScale, SpawnScaleUpSpeed);
        }
        else{
            this.transform.localScale = origScale;
        }   

        Vector3 eulerRotation = Vector3.forward * Random.Range(InitRotationMinMax.x, InitRotationMinMax.y);
        this.transform.rotation = Quaternion.Euler(eulerRotation);
        rotationSpeed = Vector3.forward * Random.Range(RotationSpeedMinMax.x,RotationSpeedMinMax.y);
        
        if(ShouldUseAlpha){
            LeanTween.value(0f, 1f, 1f).setOnUpdate((float a) 
                => SpriteRend.color = new Color(SpriteRend.color.r, SpriteRend.color.g, SpriteRend.color.b, a));
        }
        Destroy(gameObject, DestroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += MoveDirection * Time.deltaTime * GameManager.instance.Speed;
        this.transform.Rotate(rotationSpeed * Time.deltaTime);

        if(ShouldUseAlpha && Mathf.Abs(this.transform.position.z) < AlphaFadeDistance){
            float alpha =  (Mathf.Abs(this.transform.position.z) + AlphaFadeOffsetPosition) / AlphaFadeDistance;
            SpriteRend.color = new Color(SpriteRend.color.r, SpriteRend.color.g, SpriteRend.color.b, alpha);
        }

    }
}
