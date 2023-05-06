using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOpenDialogueWindow : MonoBehaviour
{
    [SerializeField] float windowOpenSpeed = 3;
    float openScale = 1;
    float closeScale = 0;
    float xWidth;
    [SerializeField] bool dialogueActive;
    RectTransform windowTransform;
    private void Awake()
    {
        dialogueActive = false;
    }
    private void Start()
    {
        windowTransform = GetComponent<RectTransform>();
        windowTransform.localScale = new Vector3(closeScale, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueActive && windowTransform.localScale.x < openScale)
        {
            xWidth = Mathf.Clamp01(windowTransform.localScale.x + Time.deltaTime * windowOpenSpeed);
            windowTransform.localScale = new Vector3(xWidth, windowTransform.localScale.y, windowTransform.localScale.z);
        }
        else if (!dialogueActive && windowTransform.localScale.x > closeScale)
        {
            xWidth = Mathf.Clamp01(windowTransform.localScale.x - Time.deltaTime * windowOpenSpeed);
            windowTransform.localScale = new Vector3(xWidth, windowTransform.localScale.y, windowTransform.localScale.z);
        }

    }
    public void ActivateDialogue(bool active)
    {
        dialogueActive = active;
    }

}
