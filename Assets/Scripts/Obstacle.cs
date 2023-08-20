using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Vector3 MoveDirection;
    public float DestoryTime = 10f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, DestoryTime);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += MoveDirection * Time.deltaTime * GameManager.instance.Speed;
    }
}
