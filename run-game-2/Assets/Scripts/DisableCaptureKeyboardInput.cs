using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCaptureKeyboardInput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        WebGLInput.captureAllKeyboardInput = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
