using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] float initHealth = 10;
    [SerializeField] GameObject deathEffect;
    [SerializeField] GameObject damageFlashArea; //the attached object will flash red when hit
    [SerializeField] float damageFlashTime = 0.05f; //length of red flash on hit
    [SerializeField] GameObject deathFadeOutUI;
    [SerializeField] UIInGameDialogue dialogueBox;
    [SerializeField] CharacterDialogue deathDialogue;
    [SerializeField] CharacterDialogue[] halfHealthDialogue;
    [SerializeField] CharacterDialogue[] almostDeadDialogue;
    bool temporarilyInvincible;
    float currentHealth = 0;
    bool playerDead;
    public static event Action OnPlayerDeath;
    private void Awake()
    {
        playerDead = false;
        currentHealth = initHealth;
    }
    public float GetHealth()
    {
        return currentHealth;
    }
    public void DealDamage(float amount)
    {
        if (temporarilyInvincible) return;
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            if (!playerDead)
            {
                Death();
                playerDead = true;
            }
            return;
        }
        else if (gameObject.name == "Player")
        {
            if (currentHealth < initHealth / 2)
            {
                for (int i = 0; i < halfHealthDialogue.Length; i++)
                {
                    dialogueBox.Dialogue(halfHealthDialogue[i]);
                }
            }
            if (currentHealth < initHealth / 10)
            {
                for (int i = 0; i < almostDeadDialogue.Length; i++)
                {
                    dialogueBox.Dialogue(almostDeadDialogue[i]);
                }
            }
        }

        StartCoroutine(FlashRed(damageFlashArea, damageFlashTime));
        temporarilyInvincible = true;
    }
    void Death()
    {
        if (gameObject.name == "Player")
        {
            StartCoroutine(PlayerDeath());
            return;
        }
        DeathEffect();
        Destroy(gameObject);
    }
    void DeathEffect()
    {
        if (deathEffect != null)
        {
            //death effect
            Instantiate(deathEffect, gameObject.transform.position, Quaternion.identity);
        }
    }
    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    IEnumerator PlayerDeath()
    {
        dialogueBox.Dialogue(deathDialogue);
        DeathEffect();
        OnPlayerDeath.Invoke();
        yield return new WaitForSeconds(3);
        deathFadeOutUI.GetComponent<UIFadeInOut>().FadeOut(1);
        yield return new WaitForSeconds(4);
        ReloadScene();
    }

    IEnumerator FlashRed(GameObject objectToFlash, float time)
    {

        Renderer renderer = objectToFlash.GetComponent<Renderer>();
        //store current material color
        Color prevColor = renderer.material.color;
        //change current color to red
        renderer.material.color = new Color(1, 0, 0);
        yield return new WaitForSeconds(time);
        //return color to stored material color
        renderer.material.color = prevColor;
        temporarilyInvincible = false;
    }
}
