using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{

    public float rotationSpeedX;
    public float rotationSpeedY;
    public float rotationSpeedZ;

    void Update()
    {
        transform.Rotate(rotationSpeedX * Time.deltaTime, rotationSpeedY * Time.deltaTime, rotationSpeedZ * Time.deltaTime);
    }
}
