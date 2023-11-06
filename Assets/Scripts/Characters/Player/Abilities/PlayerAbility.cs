using UnityEngine;
using System.Collections;

public class PlayerAbility : MonoBehaviour
{
    [SerializeField] public GameObject objectAttack;

    [SerializeField] public int actualLevel = 1, maxLevel = 10;
    [SerializeField] public float actualExp = 0, targetExp = 15;

    // Tiempo entre activaciones de la habilidad.
    [SerializeField] public float damage = 0;
    [SerializeField] public float cooldown = 0;

    protected virtual void Start()
    {
        // Llama al m�todo Activate cada "cooldown" segundos y lo repite continuamente.
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

    private IEnumerator RepeatActivation()
    {
        while (true) // Repetir indefinidamente
        {
            yield return new WaitForSeconds(cooldown);
            Activate(); // Activa la habilidad
        }
    }

    // M�todo que define el comportamiento de la habilidad.
    protected virtual IEnumerator ActivateAbility()
    {
        // Implementa el comportamiento espec�fico de la habilidad en las clases derivadas.
        yield return null;
    }

    // M�todo para activar la habilidad.
    public void Activate()
    {
        if (!IsOnCooldown)
        {
            StartCoroutine(ActivateAbility());
            StartCoroutine(Cooldown());
        }
    }

    // M�todo para controlar el tiempo de recuperaci�n.
    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
    }

    // Propiedad para verificar si la habilidad est� en tiempo de recuperaci�n.
    public bool IsOnCooldown
    {
        get { return IsInvoking("Cooldown"); }
    }
}