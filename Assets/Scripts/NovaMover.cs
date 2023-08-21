using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaMover : MonoBehaviour
{
    // Start is called before the first frame update
    public float ExpandSpeed;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale = this.transform.localScale + (Vector3.one * this.ExpandSpeed * Time.deltaTime);
    }
}
