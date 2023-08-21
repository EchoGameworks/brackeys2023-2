using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Vector3 MoveDirection;
    public float DestroyTime = 20f;
    Vector3 origScale;

    // Start is called before the first frame update
    void Start()
    {
        origScale = this.transform.localScale;
        this.transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, origScale, 0.3f);
        Destroy(gameObject, DestroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += MoveDirection * Time.deltaTime * GameManager.instance.Speed;
    }
}
