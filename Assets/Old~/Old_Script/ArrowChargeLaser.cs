using System;
using UnityEngine;
using static UnityEngine.UI.Image;

public class ArrowChargeLaser : MonoBehaviour
{


    [SerializeField]
    private Transform rayOrigin;
    [SerializeField]
    private Transform laserPoint;

    private ArrowController arrowController;
    public void Initialize(ArrowController arrowController)
    {
        this.arrowController = arrowController;
    }

    private void LateUpdate()
    {

        if (arrowController.dashState == ArrowController.DashState.Charging)
        {
            Physics.Raycast(rayOrigin.position, transform.forward, out var hitInfo, 50f);

            if (hitInfo.collider != null)
            {
                var pos = hitInfo.point;
                laserPoint.gameObject.SetActive(true);
                laserPoint.position = pos;
            }
            else
            {
                laserPoint.gameObject.SetActive(false);
            }
        }
        else
        {
            laserPoint.gameObject.SetActive(false);
        }
     
    }



}
