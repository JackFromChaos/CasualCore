using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCursorView : MonoBehaviour
{
    public GameObject handUp;
    public GameObject handDown;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
        handUp.SetActive(Input.GetMouseButton(0));
        handDown.SetActive(!Input.GetMouseButton(0));
    }
}
