using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoveringEnemy : MonoBehaviour
{
    [Header("Manuever Type")]
    [SerializeField] ManueverType primaryManeuver;
    [SerializeField] PrimaryWeapon primaryWeapon = PrimaryWeapon.Missile;
    [SerializeField] bool isPausedOnStart = false;
    [SerializeField] float pauseTimeOnStart = 0;

    [Header("Movement")]
    [SerializeField] float rotationSpeed;
    [SerializeField] float forwardSpeed;
    [SerializeField] Vector3 offsetFromTarget;
    [SerializeField] float followSpeed;
    [SerializeField] Vector3[] movePath; //Used for Maneuver: MoveBetweenPositions
    int movePathIndex = 0;
    [SerializeField] float moveBetweenIntervalTime = 1;
    float zPos;
    bool moveCoroutineRunning;
    Transform player;

    enum PrimaryWeapon
    {
        Missile,
        Lazer
    }


    enum ManueverType
    {
        StayWithTarget,
        MeleeAttackTarget,
        MoveBetweenDefinedPositions,
        MoveBetweenRandomPositions,
        MoveBetweenPositionsThenLazerSweep,
    }

    void Start()
    {
        player = GameObject.Find("Player").transform;
        zPos = player.position.z + offsetFromTarget.z;
        if (isPausedOnStart) StartCoroutine(PauseOnStart(pauseTimeOnStart));
    }
    // Update is called once per frame
    void Update()
    {
        if (isPausedOnStart) return;
        UseBaseManeuver(primaryManeuver);
    }
    void UseBaseManeuver(ManueverType maneuver)
    {
        if (player == null) return;
        switch (maneuver)
        {
            case ManueverType.MeleeAttackTarget:
                //MeleeAttackTarget();
                break;
            case ManueverType.MoveBetweenRandomPositions:
                StayWithTargetZ(player, offsetFromTarget.z, forwardSpeed);
                if (!moveCoroutineRunning)
                    StartCoroutine(MoveBetweenRandomPositions(movePath, moveBetweenIntervalTime));
                break;
            case ManueverType.MoveBetweenDefinedPositions:
                StayWithTargetZ(player, offsetFromTarget.z, forwardSpeed);
                if (!moveCoroutineRunning)
                    StartCoroutine(MoveBetweenDefinedPositions(movePath, moveBetweenIntervalTime));
                break;
            case ManueverType.MoveBetweenPositionsThenLazerSweep:
                StayWithTargetZ(player, offsetFromTarget.z, forwardSpeed);
                if (!moveCoroutineRunning)
                    StartCoroutine(MoveBetweenPositionsThenLazerSweep(moveBetweenIntervalTime));
                break;
            case ManueverType.StayWithTarget:
                StayWithTarget(player, offsetFromTarget, followSpeed);
                break;
            default:
                Debug.LogError("Invalid maneuver type");
                break;
        }
    }

    IEnumerator MoveBetweenDefinedPositions(Vector3[] offsets, float time)
    {
        while (true)
        {
            moveCoroutineRunning = true;
            while (movePathIndex < movePath.Length)
            {
                Vector2 pathPos = new Vector2(offsets[movePathIndex].x, offsets[movePathIndex].y);
                while (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), pathPos) > 1f)
                {
                    Vector3 lerpTarget = new Vector3(offsets[movePathIndex].x, offsets[movePathIndex].y, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, lerpTarget, followSpeed);
                    transform.LookAt(player);
                    yield return null;
                }

                movePathIndex++;
                transform.LookAt(player);

                yield return new WaitForSeconds(time);

                if (primaryWeapon == PrimaryWeapon.Missile)
                    GetComponent<MissileLauncher>().Fire();
                if (primaryWeapon == PrimaryWeapon.Lazer)
                    StartCoroutine(GetComponent<MissileLauncher>().FireLazer(0.1f, 0.5f, false));

                yield return new WaitForSeconds(time);

            }
            movePathIndex = 0;
            yield return null;
        }
    }
    IEnumerator MoveBetweenRandomPositions(Vector3[] offsets, float time)
    {
        while (true)
        {
            moveCoroutineRunning = true;
            int rndIntX = UnityEngine.Random.Range(-25, 26);
            int rndIntY = UnityEngine.Random.Range(-6, 13);
            Vector2 pathPos = new Vector2(rndIntX + offsetFromTarget.x, rndIntY + offsetFromTarget.y);

            while (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), pathPos) > 1f)
            {
                Vector3 lerpTarget = new Vector3(pathPos.x, pathPos.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, lerpTarget, followSpeed);
                transform.LookAt(player);
                yield return null;
            }

            transform.LookAt(player);
            yield return new WaitForSeconds(.15f);

            transform.LookAt(player);
            if (primaryWeapon == PrimaryWeapon.Missile)
                GetComponent<MissileLauncher>().Fire();
            if (primaryWeapon == PrimaryWeapon.Lazer)
                StartCoroutine(GetComponent<MissileLauncher>().FireLazer(0.1f, .9f, false));

            yield return new WaitForSeconds(time);
        }
    }
    IEnumerator MoveBetweenPositionsThenLazerSweep(float time)
    {
        while (true)
        {
            moveCoroutineRunning = true;
            int rndIntX = UnityEngine.Random.Range(-12, 13);
            int rndIntY = UnityEngine.Random.Range(-12, 13);
            Vector2 pathPos = new Vector2(rndIntX, rndIntY);
            float fireTimeMax = 1f;
            float fireTimeElapsed = 0f;
            while (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), pathPos) > .5f)
            {
                Vector3 lerpTarget = new Vector3(pathPos.x, pathPos.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, lerpTarget, followSpeed);
                transform.LookAt(player);
                yield return null;
            }
            transform.LookAt(player.position + Vector3.right * 10);
            yield return new WaitForSeconds(0.15f);
            GetComponent<MissileLauncher>().FireLazer();
            yield return new WaitForSeconds(1f);
            Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);
            while (Quaternion.Angle(transform.rotation, targetRotation) > -5f && fireTimeElapsed < fireTimeMax)
            {
                targetRotation = Quaternion.LookRotation(player.position - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                fireTimeElapsed += time / 300;
                yield return null;
            }
            yield return new WaitForSeconds(time);
            GetComponent<MissileLauncher>().DestroyLazer();
        }
    }

    private void ExitGamePlayField(bool towardsPlayer, Vector3 direction)
    {

        Vector3 targetPosition = transform.position + direction;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, forwardSpeed * Time.deltaTime);
    }

    // private void MeleeAttackTarget()
    // {
    //     return null;
    // }

    private void StayWithTarget(Transform target, Vector3 targetOffset, float followSpeed)
    {
        Vector3 targetPosition = target.position + targetOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
    private void StayWithTargetZ(Transform target, float targetOffset, float followSpeed)
    {
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, target.position.z + targetOffset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    IEnumerator PauseOnStart(float pauseTime)
    {
        yield return new WaitForSeconds(pauseTime);
        isPausedOnStart = false;
    }

}

