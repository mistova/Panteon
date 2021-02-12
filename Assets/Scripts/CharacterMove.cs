using System;
using System.Collections;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float speedForward;
    public float rotationSpeedY;
    public float speedX;
    public float jumpForce;
    float extras = 0f;

    bool getBack, canGo, endGame, screenClick;

    public Transform cameraTransform;

    CharacterController controller;

    Vector3 startingPosition;

    public Transform [] tr;

    Animator anim;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        startingPosition = tr[0].position;
        getBack = false;
        canGo = true;
        endGame = false;
        screenClick = false;
    }

    void Update()
    {
        if (canGo)
            Move();
    }

    private void LateUpdate()
    {
        if (getBack || transform.position.y < -10)
            StartCoroutine(GoToPlace());
        if (endGame)
            StartCoroutine(EndGameAsync());
    }

    private bool IsMouseInScreen()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width)
            return false;
        if (Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
            return false;
        return true;
    }

    void Movement()//Character movement with keyboard.
    {
        Vector3 locVel = Vector3.zero;
        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
            anim.SetBool("Jump", true);
        if (Input.GetKey(KeyCode.A))
            locVel.x = speedX * Time.deltaTime * (-1);
        else if (Input.GetKey(KeyCode.D))
            locVel.x = speedX * Time.deltaTime;
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("Run", true);
            locVel.z = speedForward * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            anim.SetBool("Run", true);
            locVel.z = speedForward * Time.deltaTime * (-0.5f);
        }
        else
        {
            anim.SetBool("Run", false);
            locVel.z = 0;
        }
        locVel.x += (extras * Time.deltaTime);
        controller.Move(transform.TransformDirection(locVel));
    }
    void Move()//Character movement with mouse.
    {
        Vector3 locVel = Vector3.zero;
        if (Input.GetMouseButtonDown(0) && IsMouseInScreen())
            screenClick = true;
        else if (Input.GetMouseButtonUp(0))
        {
            screenClick = false;
            anim.SetBool("Run", false);
        }
        float x;
        x = Input.mousePosition.x;
        if (x > Screen.width)
            x = Screen.width;
        else if (x < 0)
            x = 0;
        if (screenClick)
        {
            locVel.x = (x - Screen.width / 2) * speedX * Time.deltaTime / Screen.width;
            anim.SetBool("Run", true);
            locVel.z = speedForward * Time.deltaTime;
        }
        locVel.x += extras * Time.deltaTime;
        controller.Move(transform.TransformDirection(locVel));
    }

    public void PushDown(float x)//Pushed by Game object
    {
        extras += x;
    }

    public void CheckPoint(int checkPoint)//Set new check point
    {
        startingPosition = tr[checkPoint].position;
    }

    public void SetAngle(float angle)
    {
        StartCoroutine(SetAngleAsync(angle));
    }

    public void EndGame()
    {
        canGo = false;
        screenClick = false;
        anim.SetBool("Run", false);
        StartCoroutine(WaitToShowRank());
    }

    IEnumerator WaitToShowRank()
    {
        GameControl.Instance.PlayerEnded();
        yield return new WaitForSeconds(4f);
        endGame = true;
    }

    IEnumerator EndGameAsync()//Set position and angle asyncrously.
    {//Only run when cameraTransform.eulerAngles.x is greater than transform.eulerAngles.x
        endGame = false;
        float temp = cameraTransform.eulerAngles.x;
        Vector3 tempVector = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        for (float i = temp; i >= transform.eulerAngles.x; i -= 0.5f)
        {
            cameraTransform.position += (tr[tr.Length - 1].position - cameraTransform.position) / (temp - transform.eulerAngles.x);
            cameraTransform.eulerAngles = new Vector3(i, transform.eulerAngles.y, transform.eulerAngles.z);
            yield return new WaitForSeconds(0.3f / (temp - transform.eulerAngles.x));
        }
        cameraTransform.position = tr[tr.Length - 1].position;
        cameraTransform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        WallCreate.Instance.CanDraw();
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

    IEnumerator GoToPlace()//Wait some time.
    {
        canGo = false;
        anim.SetBool("Run", false);
        transform.position = startingPosition;
        yield return new WaitForSeconds(0.5f);
        getBack = false;
        canGo = true;
    }

    private void OnTriggerEnter(Collider other)//When an object triggered. 
    {
        if (other.gameObject.tag.Equals("Obstacles"))
            getBack = true;
        else if (other.gameObject.tag.Equals("RotatingPlatform"))
            PushDown(other.gameObject.GetComponent<PushPlayer>().x);
    }

    private void OnTriggerExit(Collider other)//When an object not triggered anymore. 
    {
        if (other.gameObject.tag.Equals("RotatingPlatform"))
            PushDown(other.gameObject.GetComponent<PushPlayer>().x * (-1));
    }

}