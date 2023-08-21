using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Vector3 MoveDirection;
    public float DestroyTime = 20f;
    Vector3 origScale;
    public Vector2 RotationSpeedMinMax;
    private Vector3 rotationSpeed;
    public Vector2 ScaleMinMax = Vector2.one;

    public List<Sprite> SpriteList;
    public SpriteRenderer SpriteRend;
    // Start is called before the first frame update
    void Start()
    {
        SpriteRend.sprite = SpriteList[Random.Range(0, SpriteList.Count)];
        origScale = this.transform.localScale * Random.Range(ScaleMinMax.x, ScaleMinMax.y);
        this.transform.localScale = Vector3.zero;
        rotationSpeed = Vector3.forward * Random.Range(RotationSpeedMinMax.x,RotationSpeedMinMax.y);
        LeanTween.scale(gameObject, origScale, 0.3f);
        Destroy(gameObject, DestroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += MoveDirection * Time.deltaTime * GameManager.instance.Speed;
        this.transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
