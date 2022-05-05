using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(DownloadFileScript.GetTrajectoryData().Time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
