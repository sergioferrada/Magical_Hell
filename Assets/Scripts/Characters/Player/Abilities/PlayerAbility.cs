using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Xml.Serialization;

[System.Serializable]
public class SkillLevelStatistics
{
    public float damage;
    public float cooldown;
    public float objectScale;

    public SkillLevelStatistics(float damage, float cooldown, float objectScale)
    {
        this.damage = damage;
        this.cooldown = cooldown;
        this.objectScale = objectScale;
    }
}

public class PlayerAbility : MonoBehaviour
{
    public Sprite IconSprite;
    public GameObject objectAttack;

    [Header("Ability Level Stats")]
    public int actualLevel = 1;
    public int maxLevel = 10;
    public float actualExp = 0, targetExp = 0;

    [Header("Ability Stats")]
    public float damage = 0;
    public float cooldown = 0;
    public float damageObjectScale = 1;

    #region UI SETTINGS
    [Header("UI Settings")]
    public GameObject PopUpDamagePrefab;
    #endregion

    protected virtual void Start()
    {
        ApplySkillLevel();
        // Llama al m�todo Activate cada "cooldown" segundos y lo repite continuamente.
        StartCoroutine(RepeatActivation());
    }

    public void AddExp()
    {
        actualExp++;
        FindFirstObjectByType<PlayerHUDController>().UpdateAbilitiesContainerHUD();
        if (actualExp >= targetExp && actualLevel < maxLevel)
        { 
            LevelUp();
        }
    }

    public virtual void LevelUp()
    {
        if (actualLevel < maxLevel)
        {
            actualLevel++;
            actualExp = 0;
            targetExp += 10;
            ApplySkillLevel();
            FindFirstObjectByType<PlayerHUDController>().UpdateAbilitiesContainerHUD();

            if (PopUpDamagePrefab != null)
            {
                var aux = Instantiate(PopUpDamagePrefab, transform.position, Quaternion.identity);
                aux.GetComponent<PopUpController>().PopUpTextSprite("Level Up", default, IconSprite);
                SoundManager.Instance.PlaySound("Level_Up");
            }
        }
    }

    protected virtual void ApplySkillLevel(){}

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
            if (RoomsManager.Instance.actualRoomState == RoomsManager.RoomState.PlayingRoom)
            {
                StartCoroutine(ActivateAbility());
                StartCoroutine(Cooldown());
            }
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

    protected Enemy FindNearestEnemy()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float minDistance = float.MaxValue;
        Enemy nearestEnemy = null;

        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}