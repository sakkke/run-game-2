using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCaptureKeyboardInput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        WebGLInput.captureAllKeyboardInput = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
