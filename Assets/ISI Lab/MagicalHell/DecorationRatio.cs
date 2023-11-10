using Commons.Optimization.Evaluator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationRatio : IRangedEvaluator
{
    public float MaxValue => throw new System.NotImplementedException();

    public float MinValue => throw new System.NotImplementedException();

    public object Clone()
    {
        return new DecorationRatio();
    }

    public float Evaluate(IOptimizable evaluable)
    {
        throw new System.NotImplementedException();
    }
}
