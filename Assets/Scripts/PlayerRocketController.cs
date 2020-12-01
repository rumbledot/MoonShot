using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerRocketController : MonoBehaviour
{
    [SerializeField]
    Transform playerInside;
    public Transform PlayerInside {
        get { return playerInside; }
    }
    [SerializeField]
    Transform playerLanding;
    public Transform PlayerLanding
    {
        get { return playerLanding; }
    }

    [SerializeField]
    GameObject TakeOffFX, BoostFX, IdleFX;

    GameController stats;
    GameObject player;
    GravityBodyController thisBody;
    [SerializeField]
    Transform followTarget;
    Rigidbody rocketRB;

    bool isLanded = true;
    float h, v, x, y, z, speed = 25f, verticalLookRotation;
    Vector3 moveDir, targetMoveAmount, moveAmount, smoothMoveVelocity;

    [SerializeField]
    private CinemachineVirtualCamera vcam1;

    private void Start()
    {
        stats = GameController.instance;
        player = stats.Player;
        thisBody = gameObject.GetComponent<GravityBodyController>();
        rocketRB = GetComponent<Rigidbody>();
        DisableFXs();
        this.enabled = false;
    }
    private void OnEnable()
    {
        DisableFXs();
    }
    private void Update()
    {
        HandleInputForRocketOperation();
        HandleInput();

        if (thisBody.Detach && stats.NearAPlanet)
        {
            GameController.instance.displayText("Planet detected... press Space to Land");
            thisBody.DetectGround();
        }
    }
    private void FixedUpdate()
    {
        rocketRB.MovePosition(rocketRB.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
    private void HandleInputForRocketOperation()
    {
        if (Input.GetKeyUp(KeyCode.Space) && stats.NearAPlanet)
        {
            if (isLanded)
            {
                thisBody.DetachBody();
                TookOffFXs();
                moveDir = new Vector3(0, 200f, 0).normalized;
                PropelRocket();
            }
            else if (!isLanded)
            {
                thisBody.AttachBody(stats.NearPlanet);
                isLanded = true;
                DisableFXs();
                stats.displayText("Press F to exit rocket");
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isLanded)
            {
                GameController.instance.displayText("Exiting rocket");
                GameController.instance.displayGuide("Player");
                player.SetActive(true);
                player.GetComponent<GravityBodyController>().AttachBody(stats.NearPlanet);
                player.transform.position = playerLanding.position;
                player.transform.parent = null;
                player.GetComponent<PlayerController>().enabled = true;
                player.GetComponent<PlayerController>().isInsideRocket = false;
                CinemachineBrain.SoloCamera = vcam1;
                this.enabled = false;
            }
        }
    }
    void HandleInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        h = Input.GetAxis("Mouse X");
        v = Input.GetAxis("Mouse Y");

        if (h != 0)
        {
            transform.Rotate(Vector3.up * h * Time.deltaTime * speed * 3, Space.Self);
        }
        if (!isLanded)
        {
            if (v != 0)
            {
                verticalLookRotation += v * Time.deltaTime * speed * 3;
                verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
                followTarget.localEulerAngles = Vector3.left * verticalLookRotation;
            }
            if ((x != 0 || z != 0))
            {
                moveDir = new Vector3(x, 0, z).normalized;
            }
            else
            {
                moveDir = new Vector3(0, 0, 0).normalized;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                transform.Rotate(Vector3.forward * Time.deltaTime * speed * 3, Space.Self);
            }
            if (Input.GetKey(KeyCode.E))
            {
                transform.Rotate(-Vector3.forward * Time.deltaTime * speed * 3, Space.Self);
            }
        }
        PropelRocket();
    }
    void PropelRocket()
    {
        targetMoveAmount = moveDir * speed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .3f);
    }
    private void DisableFXs()
    {
        IdleFX.SetActive(true);
        TakeOffFX.SetActive(false);
        BoostFX.SetActive(false);
    }
    private void TookOffFXs()
    {
        IdleFX.SetActive(false);
        TakeOffFX.SetActive(true);
        BoostFX.SetActive(true);
        isLanded = false;
    }
}
