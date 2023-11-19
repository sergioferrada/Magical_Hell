using Commons.Optimization.Evaluator;
using LBS.Components.Specifics;
using LBS.Components.TileMap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class TrapDistribution : IRangedEvaluator
{
    public float MaxValue => 1;

    public float MinValue => 0;

    [SerializeField, SerializeReference]
    public LBSCharacteristic SpawnCharacteristic;

    [SerializeField, SerializeReference]
    public LBSCharacteristic trapsCharacteristic;

    public float Evaluate(IOptimizable evaluable)
    {
        var chrom = evaluable as BundleTilemapChromosome ?? throw new Exception("Wrong Chromosome Type");
        float fitness = 0;

        var genes = chrom.GetGenes().Cast<BundleData>().ToList();

        List<int> spawnersPos = new List<int>();
        List<int> trapsPos = new List<int>();

        for (int i = 0; i < genes.Count; i++)
        {
            if (genes[i] != null)
            {
                if (genes[i].Characteristics.Contains(SpawnCharacteristic))
                {
                    spawnersPos.Add(i);
                }

                if (genes[i].Characteristics.Contains(trapsCharacteristic))
                {
                    trapsPos.Add(i);
                }
            }
        }

        if (spawnersPos.Count < 1)
        {
            Debug.LogWarning("Map is not suitable for the evaluation, spawners count: " + spawnersPos.Count() + " < 1");
            return MaxValue;
        }

        if (trapsPos.Count < 1)
        {
            Debug.LogWarning("Map is not suitable for the evaluation, traps count: " + trapsPos.Count() + " < 1");
            return MaxValue;
        }

        float minDist = 4.5f;

        foreach (var trap in trapsPos)
        {
            foreach(var spawn in spawnersPos)
            {
                if (trap == spawn)
                    continue;

                //Caluclar distancia
                var dist = FlatDistance(trap, spawn, chrom);

                //Comparar distancia con un minimo para aumentar el fitness
                if (dist > minDist)
                {
                    fitness++;
                }
                else
                {
                    fitness--;
                }  
            }
        }

        foreach (var trap in trapsPos)
        {
           // if (/* Ver si parte de la trampa se sale del mapa*/) fitness--;
        }

        return fitness;
    }

    public float FlatDistance(int first, int second, BundleTilemapChromosome chrom)
    {
        return (chrom.ToMatrixPosition(first) - chrom.ToMatrixPosition(second)).magnitude;
    }

    public object Clone()
    {
        var e = new TrapDistribution();
        e.SpawnCharacteristic = SpawnCharacteristic;
        e.trapsCharacteristic = trapsCharacteristic;
        return e;
    }
}
