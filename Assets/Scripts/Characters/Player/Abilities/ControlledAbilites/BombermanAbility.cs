using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BombermanAbility : PlayerAbility
{

    public GameObject bomb;

    protected override void Start()
    {
        auto = false;
        base.Start();
    }

    protected override IEnumerator ActivateAbility()
    {
        Instantiate(bomb, transform);
        return base.ActivateAbility();
    }
}
