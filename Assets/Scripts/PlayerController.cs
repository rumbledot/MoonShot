using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    float camRayLength = 100f;

    float x, y, z;
    bool isJump = false,
        isGrounded,
        isNearRocket = false,
        isOutOfBound = false;
    public bool isInsideRocket = false;

    float timeToDeath = 500f;
    float timeToDeathCounter = 0f;

    float speed = 12f;
    float h, v, verticalLookRotation;
    Vector3 moveDir, targetMoveAmount, moveAmount, smoothMoveVelocity;
    bool isMove = false;

    [SerializeField]
    Transform followTarget;

    [SerializeField]
    Transform detectGround, gunPoint;
    [SerializeField]
    LayerMask floorMask;
    [SerializeField]
    GameObject bullet;

    GravityBodyController thisBody;
    Rigidbody playerRB;

    [SerializeField]
    CinemachineVirtualCamera vcam1;
    [SerializeField]
    CinemachineVirtualCamera vcam2;

    GameController stats;
    Animator anim;

    private void Start()
    {
        thisBody = GetComponent<GravityBodyController>();
        playerRB = gameObject.GetComponent<Rigidbody>();
        stats = GameController.instance;
        anim = gameObject.GetComponent<Animator>();
        CinemachineBrain.SoloCamera = vcam1;
        GameController.instance.displayGuide("Player");
    }

    private void Update()
    {
        HandleInputForRocketOperation();
        HandleInput();
        PlayerGrounded();
        Jump();
        Animate();
        CheckOutOfBound();
    }
    private void FixedUpdate()
    {
        playerRB.MovePosition(playerRB.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
    private void HandleInputForRocketOperation()
    {
        if (Input.GetKeyDown(KeyCode.E) && isNearRocket)
        {
            stats.displayText("Boaring rocket");
            GameController.instance.displayGuide("Rocket");
            thisBody.DetachBody();
            gameObject.transform.position = stats.Rocket.GetComponent<PlayerRocketController>().PlayerInside.position;
            gameObject.transform.parent = stats.Rocket.transform;
            isInsideRocket = true;
            stats.displayText("Press SPACE to take off");
            CinemachineBrain.SoloCamera = vcam2;
            stats.Rocket.GetComponent<PlayerRocketController>().enabled = true;
            gameObject.SetActive(false);
        }
    }
    void HandleInput()
    {
        if (Input.GetMouseButton(0))
        {
            var b = Instantiate(bullet, gunPoint.position, transform.localRotation);
            b.transform.SetParent(gunPoint.transform);
            if (thisBody.Detach)
            {
                Destroy(b.GetComponent<GravityBodyController>());
            }
            else
            {
                b.GetComponent<GravityBodyController>().TheSurface = thisBody.TheSurface;
            }
            b.GetComponent<Rigidbody>().AddForce(gunPoint.transform.up * 1000f);
        }

        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        h = Input.GetAxis("Mouse X");
        v = Input.GetAxis("Mouse Y");

        if (h != 0 || v != 0)
        {
            transform.Rotate(Vector3.up * h * Time.deltaTime * speed * 3, Space.Self);
            verticalLookRotation += v * Time.deltaTime * speed * 2;
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -10, 45);
            followTarget.localEulerAngles = Vector3.left * verticalLookRotation;
        }
        if (x != 0 || z != 0)
        {
            moveDir = new Vector3(x, 0, z).normalized;
            isMove = true;
        }
        else
        {
            moveDir = new Vector3(0, 0, 0).normalized;
            isMove = false;
        }
        targetMoveAmount = moveDir * speed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .3f);
    }
    void PlayerGrounded()
    {
        isGrounded = Physics.OverlapSphere(detectGround.position, 0.3f, floorMask).Length > 0;
    }
    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded && !isJump && !thisBody.Detach)
        {
            anim.SetBool("Jumping", true);
            isJump = true;
        }
        if (!isGrounded && isJump)
        {
            anim.SetBool("Jumping", false);
            isJump = false;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("KEN-Jump") &&
            anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
        {
            anim.SetBool("Jumping", false);
            isJump = false;
        }
    }
    void Animate()
    {
        if (isMove)
        {
            anim.SetBool("Running", true);
            isMove = false;
        }
        else
        {
            anim.SetBool("Running", false);
        }
    }
    private void CheckOutOfBound()
    {
        if (isOutOfBound)
        {
            timeToDeathCounter++;
            stats.displayText("Time to imminent death " + timeToDeathCounter.ToString());
            if (timeToDeathCounter >= timeToDeath)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "PlayerRocket")
        {
            isNearRocket = true;
            stats.displayText("press 'E' to enter rocket");
        }
        if (col.gameObject.tag == "Universe")
        {
            isOutOfBound = false;
            timeToDeathCounter = 0;
            stats.displayText("back to safety");
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "PlayerRocket")
        {
            isNearRocket = false;
        }
        if (col.gameObject.tag == "Universe")
        {
            isOutOfBound = true;
            stats.displayText("turn back or DIE!");
        }
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "PlayerRocket")
        {
            isNearRocket = true;
            stats.displayText("press 'E' to enter rocket");
        }
    }
    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "PlayerRocket")
        {
            isNearRocket = false;
        }
    }

}
