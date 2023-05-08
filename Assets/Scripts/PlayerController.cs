using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class PlayerController : MonoBehaviour
{
    [Header("Cursor")]
    [SerializeField] Texture2D defaultCursor;
    [SerializeField] Texture2D enemyTargetCursor;
    Vector3 aimPoint;//Position of reticle
    Vector2 cursorHotSpot; //Centers the sprite onto the cursor
    [SerializeField] LayerMask enemyLayers; //Set layers to anything that should turn the cursor red

    [Header("CameraControl")]
    [SerializeField] CinemachineTargetGroup cameraAimGroup;
    [SerializeField] CinemachineTargetGroup cameraAimGroupClose;
    [SerializeField] GameObject cameraTarget;
    [SerializeField] bool immediateAim;
    Vector3 turretPoint;
    [SerializeField] CinemachineVirtualCamera virtualCam;
    Vector3 CameraFollowOffsetMain = new Vector3(0, 6, -15);
    Vector3 CameraFollowOffsetClose = new Vector3(0, 1, -2);

    [Header("Turret")]

    [SerializeField] GameObject[] turrets;
    [SerializeField] float turretRange;
    [SerializeField] float rotationSpeed;
    [SerializeField] MinigunRotation minigunSpin;
    [SerializeField] bool disableControls;


    private void Start()
    {
        cursorHotSpot = new Vector2(defaultCursor.width / 2, defaultCursor.height / 2);
    }
    private void OnEnable()
    {
        Health.OnPlayerDeath += DisablePlay;
    }
    private void OnDisable()
    {
        Health.OnPlayerDeath -= DisablePlay;
    }
    // Update is called once per frame
    void Update()
    {
        CustomCursor();
        if (disableControls) return;
        TurretControl();
        FireControl();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            AimDownSights(true);
        }
        else AimDownSights(false);
        if (Input.GetKey(KeyCode.A))
        {
            Time.timeScale = 4;
        }
        else Time.timeScale = 1;

    }
    void FireControl()
    {
        if (Input.GetMouseButtonDown(0))
            minigunSpin.ActivateRotation(true);
        else if (Input.GetMouseButtonUp(0))
            minigunSpin.ActivateRotation(false);
    }

    private void AimDownSights(bool active)
    {
        if (active)
        {
            virtualCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = CameraFollowOffsetClose;
        }
        else
        {
            virtualCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = CameraFollowOffsetMain;
        }
    }

    void TurretControl()
    {
        foreach (GameObject turret in turrets)
        {
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, turretRange));
            Vector3 turretDirection = targetPosition - turret.transform.position;
            Ray ray = new Ray(turret.transform.position, transform.forward);
            RaycastHit turretHit;

            if (Physics.Raycast(GetMouseRay(), out turretHit, turretRange, enemyLayers))
                turretPoint = turretHit.point;
            else turretPoint = turretDirection;

            cameraTarget.transform.position = turretPoint;

            if (immediateAim) turret.transform.LookAt(turretPoint);
            else
            {
                Quaternion targetRotation = Quaternion.LookRotation(turretPoint - turret.transform.position);
                turret.transform.rotation = Quaternion.RotateTowards(turret.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }
    void CustomCursor()
    {
        RaycastHit hit;
        if (Physics.SphereCast(GetMouseRay(), 3, out hit, 200, enemyLayers)) //Cursor over enemy
        {
            Cursor.SetCursor(enemyTargetCursor, cursorHotSpot, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(defaultCursor, cursorHotSpot, CursorMode.Auto);
        }
    }
    private static Ray GetMouseRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
    void DisablePlay()
    {
        disableControls = true;
    }
}
