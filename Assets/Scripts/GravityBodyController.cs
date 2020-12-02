using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBodyController : MonoBehaviour
{
    public GravityController TheSurface;
    public bool keepUpright = true, isStatic = true;

    [SerializeField]
    public GameObject foot = null;
    [SerializeField]
    LayerMask layerMask;

    bool isDetach, isGrounded;
    public bool Detach
    {
        get { return isDetach; }
        set { isDetach = value; }
    }
    private Rigidbody body;
    private void Awake()
    {
        if (TheSurface == null)
        {
            TheSurface = GameObject.FindGameObjectWithTag("TheSurface").GetComponent<GravityController>();
        }
        isDetach = false;
    }
    private void Start()
    {
        body = GetComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.FreezeRotation;
        body.useGravity = false;

        if (!isDetach)
        {
            TheSurface.KeepUpright(transform, keepUpright);
        }
    }
    private void Update()
    {
        if (!isDetach && !isStatic && TheSurface)
        {
            TheSurface.KeepUpright(transform, keepUpright);
        }
        else if (TheSurface == null)
        {
            DetachBody();
        }
    }
    public void DetachBody()
    {
        TheSurface = null;
        isDetach = true;
    }
    public void AttachBody(GravityController planet)
    {
        TheSurface = planet;
        isDetach = false;

        if (foot != null)
            GroundedObject();
    }
    void GroundedObject()
    {
        DetectGround();

        if (!isGrounded)
        {
            Ray ray1, ray2;
            RaycastHit hit;
            Vector3 footToCore;
            Vector3 hitToCore;

            ray1 = new Ray(transform.position, -transform.up);
            ray2 = new Ray(transform.position, transform.up);
            
            if (Physics.Raycast(ray1, out hit, 6f, layerMask) == true)
            {
                footToCore = foot.transform.position - GetComponent<GravityBodyController>().TheSurface.transform.position;
                hitToCore = hit.point - GetComponent<GravityBodyController>().TheSurface.transform.position;
                if (footToCore.magnitude > hitToCore.magnitude)
                {
                    transform.position -= (footToCore - hitToCore);
                    isGrounded = true;
                }
            }
            else if (Physics.Raycast(ray2, out hit, 6f, layerMask) == true)
            {
                footToCore = foot.transform.position - GetComponent<GravityBodyController>().TheSurface.transform.position;
                hitToCore = hit.point - GetComponent<GravityBodyController>().TheSurface.transform.position;
                if (footToCore.magnitude < hitToCore.magnitude)
                {
                    transform.position += (hitToCore - footToCore) - (hit.point - transform.position);
                    isGrounded = true;
                }
            }
        }
    }
    public void DetectGround()
    {
        Ray ray = new Ray(foot.transform.position, -foot.transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 0.01f, layerMask) == true)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    public void destroySelf()
    {
        if (Application.isEditor)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
