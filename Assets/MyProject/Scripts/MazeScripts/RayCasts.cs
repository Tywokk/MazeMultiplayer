using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCasts : MonoBehaviour
{
    private float distanceRay = 50;
    [SerializeField] private LayerMask objectSelectionMask;

    private Transform startObj;
    void Start()
    {
        startObj = GetComponent<Transform>();
    }
    private void OnRenderObject()
    {
        RaycastInfo();
    }
    private void RaycastInfo()
    {
        Ray ray = new Ray(startObj.position + startObj.right * 5, startObj.up * -1);
        Ray ray2 = new Ray(startObj.position + startObj.right * -4.8f, startObj.up * -1);
        //Ray ray3 = new Ray(startObj.position + startObj.forward * -5f, startObj.up * -1);
        //Debug.DrawRay(startObj.position + startObj.right * 5, startObj.up * -1 * distanceRay);
        //Debug.DrawRay(startObj.position + startObj.right * -4.8f, startObj.up * -1 * distanceRay);
        //Debug.DrawRay(startObj.position + startObj.forward * -4.8f, startObj.up * -1 * distanceRay);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, distanceRay, objectSelectionMask))
        {
            hitInfo.transform.gameObject.SetActive(false);
        }
        if (Physics.Raycast(ray2, out RaycastHit hitInfo2, distanceRay, objectSelectionMask))
        {
            GameObject wallLeft = hitInfo2.transform.gameObject.GetComponentInParent<Cell>().WallLeft;
            GameObject wallBottom = hitInfo2.transform.gameObject.GetComponentInParent<Cell>().WallBottom;
            if (hitInfo2.transform.gameObject == wallLeft && wallLeft.GetComponent<MeshRenderer>().enabled == false)
            {
                hitInfo2.transform.gameObject.SetActive(false);
            }
            if (hitInfo2.transform.gameObject == wallBottom && wallBottom.GetComponent<MeshRenderer>().enabled == false)
            {
                hitInfo2.transform.gameObject.SetActive(false);
            }

        }
        /*if (Physics.Raycast(ray3, out RaycastHit hitInfo3, distanceRay, objectSelectionMask))
        {
            GameObject wallLeft = hitInfo2.transform.gameObject.GetComponentInParent<Cell>().WallLeft; 
            GameObject wallBottom = hitInfo2.transform.gameObject.GetComponentInParent<Cell>().WallBottom;
            if (hitInfo2.transform.gameObject == wallLeft && wallLeft.GetComponent<MeshRenderer>().enabled == false)
            {
                hitInfo2.transform.gameObject.SetActive(false);
            }
            if (hitInfo2.transform.gameObject == wallBottom && wallBottom.GetComponent<MeshRenderer>().enabled == false)
            {
                hitInfo2.transform.gameObject.SetActive(false);
            }

        
        }*/
    }
}
