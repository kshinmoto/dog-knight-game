using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("d"))
        {
            VirtualInputManager.Instance.MoveRight = true;
        }
        else
        {
            VirtualInputManager.Instance.MoveRight = false;
        }

        if (Input.GetKey("a"))
        {
            VirtualInputManager.Instance.MoveLeft = true;
        }
        else
        {
            VirtualInputManager.Instance.MoveLeft = false;
        }

        /*
        if (VirtualInputManager.Instance.MoveRight)
        {
            this.gameObject.transform.Translate(Vector3.forward * 10f * Time.deltaTime);
        }

        if (VirtualInputManager.Instance.MoveLeft)
        {
            this.gameObject.transform.Translate(-Vector3.forward * 10f * Time.deltaTime);
        }
        */
    }
}
