using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] Vector3 rotation;
    List<GameObject> children = new List<GameObject>();
    List<Vector3> offsets = new List<Vector3>();
    int childCount;
    private void Start()
    {
        childCount = transform.childCount;
        StartCoroutine(RemoveChildren());
    }

    void Update()
    {

    }
    IEnumerator RemoveChildren()
    {
        yield return new WaitForSeconds(.4f);
        for (int i = 0; i < childCount; i++)
        {
            children.Add(transform.GetChild(i).gameObject);
        }
        foreach (GameObject child in children)
        {
            child.transform.SetParent(null);
        }
        yield return null;
    }
}
