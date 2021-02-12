using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    public int checkPoint;
    public int checkPointTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            other.gameObject.GetComponent<CharacterMove>().CheckPoint(checkPoint);
            other.gameObject.GetComponent<CharacterMove>().SetAngle(transform.eulerAngles.y);
        }
        else if (other.gameObject.tag.Equals("Opponent"))
        {
            other.gameObject.GetComponent<OpponentMovement>().CheckPoint(checkPoint, checkPointTarget);
            other.gameObject.GetComponent<OpponentMovement>().SetAngle(transform.eulerAngles.y);
        }
    }
}
