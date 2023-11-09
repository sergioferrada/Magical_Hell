using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerAbility : MonoBehaviour
{
    [SerializeField] private Image Icon;
    [SerializeField] public Sprite IconSprite;
    [SerializeField] public GameObject objectAttack;
    
    [SerializeField] public int actualLevel = 1, maxLevel = 10;
    [SerializeField] public float actualExp = 0, targetExp = 15;

    // Tiempo entre activaciones de la habilidad.
    [SerializeField] public float damage = 0;
    [SerializeField] public float cooldown = 0;
    [SerializeField] public float damageObjectScale = 1;

    protected virtual void Start()
    {
        // Llama al método Activate cada "cooldown" segundos y lo repite continuamente.
        StartCoroutine(RepeatActivation());
    }

    public Image getIcon() { return Icon; }

    public void AddExp()
    {
        actualExp++;
        GetComponent<AbilitiesContainerController>().UpdateAbilitiesContainerHUD();
        if (actualExp >= targetExp && actualLevel < maxLevel)
        { 
            LevelUp();
        }
    }

    public virtual void LevelUp()
    {
        actualLevel++;
        actualExp = 0;
        targetExp += 10;
        damage *= 1.1f;
        cooldown *= 0.95f;
        damageObjectScale += 0.5f;

        GetComponent<AbilitiesContainerController>().UpdateAbilitiesContainerHUD();
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