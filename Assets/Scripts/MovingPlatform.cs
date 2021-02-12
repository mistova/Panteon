using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    Vector3 startingPosition;
    public float speedX;
    public float speedZ;
    public float speedY;
    public float left, right;

    bool direction;
    void Start()
    {
        startingPosition = transform.position;
        direction = true;
    }
    void Update()
    {
        if (transform.position.x - startingPosition.x > right || transform.position.y - startingPosition.y > right || transform.position.z - startingPosition.z > right)
            direction = false;
        else if (transform.position.x - startingPosition.x < left  || transform.position.y - startingPosition.y < left  || transform.position.z - startingPosition.z < left)
            direction = true;
        if(direction)
            transform.position = new Vector3(transform.position.x + speedX * Time.deltaTime, transform.position.y + speedY * Time.deltaTime, transform.position.z + speedZ * Time.deltaTime);
        else
            transform.position = new Vector3(transform.position.x - speedX * Time.deltaTime, transform.position.y - speedY * Time.deltaTime, transform.position.z - speedZ * Time.deltaTime);
    }
}
