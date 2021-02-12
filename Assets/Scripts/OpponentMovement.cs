using System;
using System.Collections;
using UnityEngine;

public class OpponentMovement : MonoBehaviour
{

    CharacterController controller;

    public string opponentName;

    public float speed;

    Vector3 startingPosition;
    int checkPointTarget;

    public Transform[] tr;
    public Transform[] targets;
    int currentTarget;

    public float lookRange;

    float extras;

    public LayerMask obstacleLayer;

    bool getBack, finished;

    Animator anim;
    void Start()
    {
        checkPointTarget = 0;
        getBack = false;
        finished = false;
        startingPosition = tr[0].position;
        currentTarget = 1;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if(currentTarget < targets.Length)
        {
            float distance = Vector3.Distance(targets[currentTarget].position, transform.position);
            float distanceX = transform.TransformDirection(targets[currentTarget].position - transform.position).x;
            Collider[] obstacles = Physics.OverlapCapsule(transform.position, transform.position + transform.TransformDirection(new Vector3(0, 0, 2f)), lookRange, obstacleLayer);
            bool wayClear = WaitObstacleToPass(ref obstacles);
            if ((distance > 0.5f || (distanceX > 1 && distance < 1.6f)) && wayClear)
            {
                Vector3 locVel = (targets[currentTarget].position - transform.position).normalized * speed * Time.deltaTime;
                locVel.x += extras * Time.deltaTime;
                controller.Move(locVel);
                anim.SetBool("Run", true);
            }
            else if (wayClear)
            {
                currentTarget++;
                if (currentTarget == targets.Length)
                    finished = true;
                anim.SetBool("Run", false);
            }
            else
                anim.SetBool("Run", false);
        }
    }

    bool WaitObstacleToPass(ref Collider[] obs)
    {
        for(int i = 0; i < obs.Length; i++)
        {
            if (transform.InverseTransformDirection(transform.position - obs[i].transform.position).z < 0)
                return false;
        }
        return true;
    }

    private void LateUpdate()
    {
        if (getBack || transform.position.y < -10)
            StartCoroutine(GoToPlace());
    }

    public float DistanceToTarget()
    {
        return Vector3.Distance(targets[currentTarget].position, transform.position);
    }
    public string GetName()
    {
        return opponentName;
    }

    public int CurrentTarget()
    {
        if (!finished)
            return currentTarget;
        else
            return -1;
    }

    public bool isFinished()
    {
        return finished;
    }

    IEnumerator GoToPlace()//Wait some time.
    {
        anim.SetBool("Run", false);
        currentTarget = checkPointTarget;
        extras = 0;
        transform.position = startingPosition;
        yield return new WaitForSeconds(0.5f);
        getBack = false;
    }

    public void CheckPoint(int checkPoint, int checkPointTarget)//Set new check point
    {
        extras = 0;
        startingPosition = tr[checkPoint].position;
        this.checkPointTarget = checkPointTarget;
    }

    IEnumerator SetAngleAsync(float angle)//Set angle to given angle asyncrously.
    {//Only run when angle is greater than transform.eulerAngles.y
        float temp = transform.eulerAngles.y;
        for (float i = temp; i <= angle; i++)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, i, transform.eulerAngles.z);
            yield return new WaitForSeconds(0.3f / (angle - temp));
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
    }
    public void SetAngle(float angle)
    {
        StartCoroutine(SetAngleAsync(angle));
    }

    private void OnTriggerEnter(Collider other)//When an object triggered. 
    {
        if (other.gameObject.tag.Equals("Obstacles"))
            getBack = true;
        else if (other.gameObject.tag.Equals("RotatingPlatform"))
            TakeAffectFromRotatingPlatform(other.gameObject.GetComponent<PushPlayer>().x);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("RotatingPlatform"))
            TakeAffectFromRotatingPlatform(other.gameObject.GetComponent<PushPlayer>().x * (-1));
    }

    private void TakeAffectFromRotatingPlatform(float affect)
    {
        System.Random random = new System.Random();
        extras += affect * ((float) random.NextDouble() * 3 / 4);
    }
}
