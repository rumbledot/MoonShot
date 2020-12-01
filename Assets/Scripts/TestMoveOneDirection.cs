using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveOneDirection : MonoBehaviour
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

    [SerializeField]
    LayerMask layer_mask;

    public GameObject pointerObj;

    bool isGrounded = false;
    Vector3 oldPos;

    float x, y, z, speed, timeToMove;
    Vector3 moveBy, moveUpBy, halfWay;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        body = GetComponent<GravityBodyController>();
        planet = body.TheSurface.transform;
        speed = 10f;

        //x = Random.Range(-0.5f, 0.5f);
        x = 0;
        z = Random.Range(0.1f, 0.3f);
    }
    private void Update()
    {
        UsingMovePosition();
        KeepGrounded();
    }
    private void UsingMovePosition()
    {
        Ray ray = new Ray(foot.position, -foot.up);
        RaycastHit hit;
        //Debug.DrawLine(foot.position, foot.position - foot.up, Color.red);
        //Debug.DrawLine(foot.position, foot.position + (foot.localPosition + new Vector3(0, -0.01f, 0)), Color.green);
        //Debug.DrawLine(foot.position, foot.position + (foot.localPosition + new Vector3(0, 0, -0.01f)), Color.blue);
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

        moveBy = (transform.right * x + transform.forward * z).normalized;
        rb.MovePosition(Vector3.Lerp(transform.position, transform.position + moveBy.normalized, speed * Time.deltaTime));
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
                Debug.DrawRay(foot.position, -foot.up * 6f, Color.black, 1);
                footToCore = foot.transform.position - GetComponent<GravityBodyController>().TheSurface.transform.position;
                hitToCore = hit1.point - planet.position;

                y = (footToCore - hitToCore).magnitude;
                transform.Translate(new Vector3(0f, -y, 0f), Space.Self);

                GetComponent<MeshRenderer>().material.color = Color.green;
                isGrounded = true;
            }
            Ray ray2 = new Ray(foot.position, foot.up);
            RaycastHit hit2;
            if (Physics.Raycast(ray2, out hit2, 6f, layer_mask) == true)
            {
                Debug.DrawRay(foot.position, foot.up * 6f, Color.white, 1);
                footToCore = foot.transform.position - GetComponent<GravityBodyController>().TheSurface.transform.position;
                hitToCore = hit2.point - planet.position;

                y = (hitToCore - footToCore).magnitude;
                transform.Translate(new Vector3(0f, y, 0f), Space.Self);

                GetComponent<MeshRenderer>().material.color = Color.green;
                isGrounded = true;
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
