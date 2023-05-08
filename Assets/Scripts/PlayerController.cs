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
    [SerializeField] MinigunRotation[] minigunSpin;
    [SerializeField] bool disableControls;
    [SerializeField] Vector3[] turretOffsets;
    bool turretsActivatedFinal;


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
        if (Input.GetKey(KeyCode.A))
        {
            Time.timeScale = 4;
        }
        else Time.timeScale = 1;
        if (GameObject.Find("Enemy").GetComponent<WaveManager>().GetWaveNum() == 6 && !turretsActivatedFinal)
        {
            GetComponent<EnergyScript>().ReplenishEnergy(50);
            foreach (GameObject turret in turrets)
            {
                if (!turret.activeInHierarchy)
                {
                    turret.SetActive(true);
                }
            }
            foreach (MinigunRotation minigun in minigunSpin)
            {
                minigun.transform.parent.gameObject.SetActive(true);
            }
            turretsActivatedFinal = true;
        }

    }
    void FireControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach (MinigunRotation minigun in minigunSpin)
            {
                if (minigun.enabled)
                    minigun.ActivateRotation(true);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            foreach (MinigunRotation minigun in minigunSpin)
            {
                if (minigun.enabled)
                    minigun.ActivateRotation(false);
            }
        }
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
        if (turrets[0])
        {
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, turretRange) + turretOffsets[0]);
            Vector3 turretDirection = targetPosition - turrets[0].transform.position;
            Ray ray = new Ray(turrets[0].transform.position, transform.forward);
            RaycastHit turretHit;

            if (Physics.Raycast(GetMouseRay(), out turretHit, turretRange, enemyLayers))
                turretPoint = turretHit.point;
            else turretPoint = turretDirection;

            cameraTarget.transform.position = turretPoint;

            if (immediateAim) turrets[0].transform.LookAt(turretPoint + turretOffsets[0]);
            else
            {
                Quaternion targetRotation = Quaternion.LookRotation(turretPoint - turrets[0].transform.position);
                turrets[0].transform.rotation = Quaternion.RotateTowards(turrets[0].transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
        if (GameObject.Find("Enemy").GetComponent<WaveManager>().GetWaveNum() == 6)
        {
            for (int i = 1; i < turrets.Length; i++)
            {
                Vector3 direction = turretPoint + turretOffsets[i] - turrets[i].transform.position;
                Quaternion newRot = Quaternion.LookRotation(direction);

                turrets[i].transform.rotation = Quaternion.Lerp(turrets[i].transform.rotation, newRot, 1 * Time.deltaTime);
            }
        }
    }
    void CustomCursor()
    {
        RaycastHit hit;
        if (Physics.SphereCast(GetMouseRay(), 3, out hit, 200, enemyLayers)) //Cursor over enemy
        {
            Cursor.SetCursor(enemyTargetCursor, cursorHotSpot, CursorMode.ForceSoftware);
        }
        else
        {
            Cursor.SetCursor(defaultCursor, cursorHotSpot, CursorMode.ForceSoftware);
        }
    }
    private static Ray GetMouseRay(Vector3 offset = default(Vector3))
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
    void DisablePlay()
    {
        disableControls = true;
    }
}
