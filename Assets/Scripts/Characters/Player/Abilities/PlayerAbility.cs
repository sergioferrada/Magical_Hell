using UnityEngine;
using System.Collections;

//[System.Serializable]
public class PlayerAbility : MonoBehaviour
{
    public int actualLevel = 1, maxLevel = 10;
    public float actualExp = 0, targetExp = 15;

    // Tiempo entre activaciones de la habilidad.
    public float damage;
    public float cooldown;

    protected virtual void Start()
    {
        // Llama al método Activate cada "cooldown" segundos y lo repite continuamente.
        StartCoroutine(RepeatActivation());
    }

    public void AddExp()
    {
        actualExp++;

        if(actualExp >= targetExp)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        if (actualLevel < maxLevel)
        {
            actualLevel++;
            actualExp = 0;
            targetExp += 10;
            damage *= 1.1f;
            cooldown *= 0.95f;
        }
        else
            return;
    }

    public virtual void SetAbilityStats(float d, float cd, GameObject obj = null)
    {
        damage = d;
        cooldown = cd;
    }

    private IEnumerator RepeatActivation()
    {
        while (true) // Repetir indefinidamente
        {
            yield return new WaitForSeconds(cooldown);
            Activate(); // Activa la habilidad
        }
    }

    // Método que define el comportamiento de la habilidad.
    protected virtual IEnumerator ActivateAbility()
    {
        // Implementa el comportamiento específico de la habilidad en las clases derivadas.
        yield return null;
    }

    // Método para activar la habilidad.
    public void Activate()
    {
        if (!IsOnCooldown)
        {
            StartCoroutine(ActivateAbility());
            StartCoroutine(Cooldown());
        }
    }

    // Método para controlar el tiempo de recuperación.
    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
    }

    // Propiedad para verificar si la habilidad está en tiempo de recuperación.
    public bool IsOnCooldown
    {
        get { return IsInvoking("Cooldown"); }
    }
}