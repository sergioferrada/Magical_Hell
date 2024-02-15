using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//el ataque melee que tiene el juego en pre-pre alpha pero como habilidad
//TODO: Cambiar y poner el leveleo. Limpiar
public class BasicMeleeAbility : PlayerAbility
{
    private PlayerController player;
    public MeleePlayerAttack attackComponent;

    protected override void Start()
    {
        if (attackComponent == null)
        {
            attackComponent = gameObject.GetComponent<MeleePlayerAttack>();
        }

        auto = false;

        player = FindObjectOfType<PlayerController>();

        base.Start();
    }

    protected override IEnumerator ActivateAbility()
    {

        attackComponent.damage = player.Damage;
        attackComponent.impulseForce = player.ImpulseForce;
        attackComponent.objectScale = player.AttackRange;
        attackComponent.Activate();

        return base.ActivateAbility();
    }
}