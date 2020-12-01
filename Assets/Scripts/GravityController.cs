using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    public float gravity = -10f;
    public void CreateUpright(Transform body)
    {
        Vector3 gravityUp = (body.position - transform.position).normalized;
        Vector3 bodyUp = body.up;
        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * body.rotation;
        body.rotation = targetRotation;

        Ray ray = new Ray(body.position, -body.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 6f) == false)
        {
            Destroy(body.gameObject);
        }
    }
    public void KeepUpright(Transform body, bool keepUpright = true)
    {
        Vector3 gravityUp = (body.position - transform.position).normalized;
        Vector3 bodyUp = body.up;

        body.GetComponent<Rigidbody>().AddForce(gravityUp * gravity);

        if (keepUpright)
        {
            Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * body.rotation;
            body.rotation = Quaternion.Slerp(body.rotation, targetRotation, 50 * Time.deltaTime);
        }
    }
    private static void DetectSurface(Transform body)
    {
        Ray ray = new Ray(body.position, -body.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10f) == true)
        {
            Quaternion rotCur = Quaternion.FromToRotation(body.up, hit.normal) * body.rotation;
            Vector3 posCur = new Vector3(body.position.x, hit.point.y, body.position.z);
        }
    }
}
