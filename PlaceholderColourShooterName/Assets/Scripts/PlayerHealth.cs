using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{

    [SyncVar(hook = "UpdateHealthBar")] private float currentHealth;
    public float maxHealth = 3;
    [SyncVar] public bool isDead = false;
    public RectTransform healthBar;

	void Start ()
    {
        Reset();
        //StartCoroutine("CountDown");
	}

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1f);
        Damage(1f);
        UpdateHealthBar(currentHealth);

        yield return new WaitForSeconds(1f);
        Damage(1f);
        UpdateHealthBar(currentHealth);

        yield return new WaitForSeconds(1f);
        Damage(1f);
        UpdateHealthBar(currentHealth);
    }
	
	void UpdateHealthBar (float value)
    {
        if (healthBar != null)
        {
            healthBar.sizeDelta = new Vector2(value / maxHealth * 450f, healthBar.sizeDelta.y);
        }
	}

    public void Damage(float damage)
    {
        if (!isServer)
        {
            return;
        }

        currentHealth -= damage;

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            RpcDie();
        }
    }

    [ClientRpc]
    void RpcDie()
    {
        SetActiveState(false);
        gameObject.SendMessage("Respawn");
    }

    void SetActiveState(bool state)
    {
        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            c.enabled = state;
        }

        foreach (Canvas c in GetComponentsInChildren<Canvas>())
        {
            c.enabled = state;
        }

        foreach (Renderer c in GetComponentsInChildren<Renderer>())
        {
            c.enabled = state;
        }
    }

    public void Reset()
    {
        currentHealth = maxHealth;
        SetActiveState(true);
        isDead = false;
    }
}
