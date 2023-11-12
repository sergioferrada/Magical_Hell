using Commons.Optimization.Evaluator;
using LBS.Components.Specifics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapDistribution : IRangedEvaluator
{
    public float MaxValue => 1;

    public float MinValue => 0;



    public object Clone()
    {
        throw new System.NotImplementedException();
    }

    public float Evaluate(IOptimizable evaluable)
    {
        throw new System.NotImplementedException();

        //foreach(var trap in traps)
        //{
        //    foreach(var spawn in spawners)
        //    {
        //        if(Vector2.Distance(spawn.transform.position, trap.transform.position) > )
        //    }
        //}
    }
}
