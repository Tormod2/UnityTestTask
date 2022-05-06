using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    [SerializeField]
    private Transform objectTransform;
    
    [SerializeField]
    private Vector3 offset;

    //Makes camera follow the object keeping the given distance from it
    private void Update()
    {
        transform.position = objectTransform.position + offset;
    }
}
