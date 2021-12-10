using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGscript : MonoBehaviour
{
    private MeshRenderer render;

    public float speed;
    private float Offset;

    void Start()
    {
        render = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Offset += Time.deltaTime * speed ;
        render.material.mainTextureOffset = new Vector2 ( Offset, 0);
    }
}
