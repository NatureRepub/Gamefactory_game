using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletpos : MonoBehaviour
{
    public static GameObject bulletpos_R;
    public static GameObject bulletpos_L;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            bulletpos_R.transform.localPosition = new Vector3(0.207f, 0.037f, 0);
            bulletpos_L.transform.localPosition = new Vector3(-0.207f, 0.037f, 0);
        }
    }
}
