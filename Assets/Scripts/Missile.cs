using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour
{
    public Transform target;
    public float speed = 80f;
    public float turnSpeed = 2f;
    Vector3 aimPoint;
    Transform rotationPoint;
    Vector3 localOffset;

    [SerializeField] MovementMode movementMode;
    public enum MovementMode
    {
        Wild,
        Straight,
        NoMovement,
        RotateAroundObject
    }

    [Header("Mode Settings: Wild")]

    [SerializeField] Vector3 posA, posB, posC, posD, posE;
    [SerializeField] float timeA, timeB, timeC, timeD, timeE;

    [Header("Mode Settings: RotateAroundObject")]
    [SerializeField] float rotationSpeed;

    private void Awake()
    {
        if (movementMode == MovementMode.RotateAroundObject)
        {
            rotationPoint = transform.parent;
            localOffset = rotationPoint.position - transform.position;
        }
    }
    private void Start()
    {
        if (target == null)
            target = GameObject.Find("Player").transform;

        if (movementMode == MovementMode.Wild)
        {
            posA = new Vector3(Random.Range(-20, 21), Random.Range(-20, 21), Random.Range(6, 10));
            posB = new Vector3(Random.Range(-20, 21), Random.Range(-20, 21), 10);
            posC = new Vector3(Random.Range(-30, 31), 0, Random.Range(6, 12));
            posD = new Vector3(0, 0, Random.Range(4, 12));
            posE = new Vector3(Random.Range(-5, 6), Random.Range(-5, 6), 20);
            StartCoroutine(WildMovement());
        }
        if (movementMode == MovementMode.Straight)
        {
            aimPoint = target.position;
        }
    }
    private void Update()
    {
        if (movementMode == MovementMode.RotateAroundObject)
        {
            RotateAroundObject(rotationPoint);
        }
        if (target == null || movementMode == MovementMode.NoMovement)
        {
            return;
        }

        // Rotate towards the target
        Vector3 dir = aimPoint - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);

        // Move towards the target
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    IEnumerator WildMovement()
    {
        aimPoint = posA;
        yield return new WaitForSeconds(timeA);
        aimPoint = posB;
        yield return new WaitForSeconds(timeB);
        aimPoint = posC;
        yield return new WaitForSeconds(timeC);
        aimPoint = posD;
        yield return new WaitForSeconds(timeD);
        aimPoint = posE;
        yield return new WaitForSeconds(timeE);
        aimPoint = target.position;
        yield return null;
    }
    void RotateAroundObject(Transform rotationPoint)
    {
        transform.RotateAround(rotationPoint.position, transform.forward, rotationSpeed);
        transform.position = rotationPoint.localPosition + localOffset;
    }
}
