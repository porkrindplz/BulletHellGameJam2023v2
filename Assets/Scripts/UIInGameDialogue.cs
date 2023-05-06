using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInGameDialogue : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI characterName;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image portrait;
    UIOpenDialogueWindow dialogueWindow;

    // Start is called before the first frame update
    void Start()
    {
        dialogueWindow = GetComponent<UIOpenDialogueWindow>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Dialogue(CharacterDialogue characterDialogue)
    {
        StartCoroutine(StartDialogue(characterDialogue));
        Debug.Log("dialogue method");
    }
    IEnumerator StartDialogue(CharacterDialogue characterDialogue)
    {
        Debug.Log("Character complete? " + characterDialogue.complete);
        if (!characterDialogue.complete)
        {
            yield return new WaitForSeconds(0.3f);
            portrait.sprite = characterDialogue.characterToTalk.character.portrait;
            characterName.text = characterDialogue.characterToTalk.character.characterName;
            text.text = characterDialogue.dialogue;
            dialogueWindow.ActivateDialogue(true);
            yield return new WaitForSeconds(characterDialogue.textTime - .6f);
            dialogueWindow.ActivateDialogue(false);
            yield return new WaitForSeconds(0.3f);
            characterDialogue.complete = true;
        }
        else yield return null;
    }
    void ClearText()
    {
        dialogueWindow.ActivateDialogue(false);
        portrait.sprite = null;
        text.text = null;
    }
}
