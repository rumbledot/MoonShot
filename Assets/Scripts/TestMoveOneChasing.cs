using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveOneChasing : MonoBehaviour
{
    enum MoveDir
    {
        dirFw, dirBc, dirLf, dirRg
    };
    MoveDir pickDir;

    public Transform foot;
    Rigidbody rb;
    GravityBodyController body;
    Transform planet;

    GameObject player;
    GravityBodyController playerBody;

    [SerializeField]
    LayerMask layer_mask;
    public GameObject pointerObj;
    bool isGrounded = false;

    float x, y, z, speed, timeToMove;
    Vector3 moveBy, halfWay;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        body = GetComponent<GravityBodyController>();
        planet = body.TheSurface.transform;
        speed = 10f;

        player = GameObject.FindGameObjectWithTag("Player");
        playerBody = player.GetComponent<GravityBodyController>();

    }
    private void Update()
    {
        if (playerBody.TheSurface == body.TheSurface)
        {
            UsingMovePosition();
            KeepGrounded();
        }
    }
    private void UsingMovePosition()
    {
        Ray ray = new Ray(foot.position, -foot.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 0.01f, layer_mask) == true)
        {
            isGrounded = true;
            GetComponent<MeshRenderer>().material.color = Color.white;
        }
        else
        {
            isGrounded = false;
            GetComponent<MeshRenderer>().material.color = Color.red;
        }

        x = (player.transform.position.x < transform.position.x) ? -1 : 1;
        y = (player.transform.position.y < transform.position.y) ? -1 : 1;
        z = (player.transform.position.z < transform.position.z) ? -1 : 1;

        moveBy = (transform.right * x + transform.forward * z).normalized;
        rb.MovePosition(Vector3.Lerp(transform.position, transform.position + moveBy, speed * Time.deltaTime));
    }
    private void KeepGrounded()
    {
        if (!isGrounded)
        {
            Ray ray;
            RaycastHit hit;
            Vector3 footToCore;
            Vector3 hitToCore;
            GetComponent<MeshRenderer>().material.color = Color.black;
            Ray ray1 = new Ray(foot.position, -foot.up);
            RaycastHit hit1;
            if (Physics.Raycast(ray1, out hit1, 6f, layer_mask) == true)
            {
                footToCore = foot.transform.position - GetComponent<GravityBodyController>().TheSurface.transform.position;
                hitToCore = hit1.point - GetComponent<GravityBodyController>().TheSurface.transform.position;
                if (footToCore.magnitude > hitToCore.magnitude)
                {
                    transform.position -= (footToCore - hitToCore);
                    GetComponent<MeshRenderer>().material.color = Color.green;
                    isGrounded = true;
                }
            }
            Ray ray2 = new Ray(foot.position, foot.up);
            RaycastHit hit2;
            if (Physics.Raycast(ray2, out hit2, 6f, layer_mask) == true)
            {
                footToCore = foot.transform.position - GetComponent<GravityBodyController>().TheSurface.transform.position;
                hitToCore = hit2.point - GetComponent<GravityBodyController>().TheSurface.transform.position;
                if (footToCore.magnitude < hitToCore.magnitude)
                {
                    transform.position += (hitToCore - footToCore) - (hit2.point - transform.position);
                    GetComponent<MeshRenderer>().material.color = Color.blue;
                    isGrounded = true;
                }
            }
        }
        
                //Ray rayDepth = new Ray(foot.position, -foot.up);
                //RaycastHit hitDepth;
                //if (Physics.Raycast(rayDepth, out hitDepth, Mathf.Infinity, layer_mask) == true)
                //{
                //var footToCore = foot.transform.position - GetComponent<GravityBodyController>().TheSurface.transform.position;
                //var hitToCore = hit.point - GetComponent<GravityBodyController>().TheSurface.transform.position;
                //var pos = transform.position + ((footToCore - hitToCore) * -1);
                //Debug.DrawLine(hit.point, gameObject.GetComponent<GravityBodyController>().TheSurface.transform.position, Color.blue, 200);
                //Debug.DrawLine(hit.point, foot.position, Color.red, 200);
                //Debug.Log("Distance : " + (hit.point - foot.position).magnitude);
                //transform.localPosition += new Vector3(0f, (hit.point - foot.position).magnitude * -1, 0f);
                //}
                //Debug.Log("Hit head");
                //var p = Instantiate(pointerObj, hit.point, Quaternion.identity);
                //p.transform.SetParent(transform);
                //Destroy(p, 0.5f);
                //halfWay = ((hit.point - foot.position) / -1).normalized;
                //Debug.Log("half way : " + halfWay);
                //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + halfWay.y, transform.localPosition.z);
        
            //Debug.DrawLine(foot.transform.position, GetComponent<GravityBodyController>().TheSurface.transform.position, Color.red, 200);
        
        //else if (Physics.Raycast(rayFootUp, out hitFootUp, 1f, layer_mask) == true)
        //{
        //    Debug.Log("Hit foot");
        //    var p1 = Instantiate(pointerObj, hitFootUp.point, Quaternion.identity);
        //    p1.transform.SetParent(transform);
        //    p1.GetComponent<MeshRenderer>().material.color = Color.red;
        //    Destroy(p1, 0.5f);
        //}
    }
    //private void OnCollisionEnter(Collision col)
    //{
    //    if (col.gameObject.tag == " TheSurface")
    //    {
    //        KeepAboveGround();
    //    }
    //}
}
