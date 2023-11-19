using Commons.Optimization.Evaluator;
using LBS.Components.TileMap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class DecorationDistribution : IRangedEvaluator
{
    public float MaxValue => 1;

    public float MinValue => 0;


    [SerializeField, SerializeReference]
    public LBSCharacteristic spawnCharacteristic;

    [SerializeField, SerializeReference]
    public LBSCharacteristic decorationCharacteristic;

    [SerializeField, SerializeReference]
    public LBSCharacteristic decoration2Characteristic;

    public float Evaluate(IOptimizable evaluable)
    {
        var chrom = evaluable as BundleTilemapChromosome ?? throw new Exception("Wrong Chromosome Type");
        float fitness = 0;

        var genes = chrom.GetGenes().Cast<BundleData>().ToList();

        List<int> spawnersPos = new List<int>();

        for (int i = 0; i < genes.Count; i++)
        {
            if (genes[i] != null)
            {
                if (genes[i].Characteristics.Contains(spawnCharacteristic))
                {
                    spawnersPos.Add(i);
                }
            }
        }

        if (spawnersPos.Count < 1)
        {
            Debug.LogWarning("Map is not suitable for the evaluation, spawners count: " + spawnersPos.Count + " = 0");
            return MaxValue;
        }

        float dist_1 = 2.0f;
        float dist_2 = 4.5f;
        int flag = 0;

        for (int i = 0; i < genes.Count; i++)
        {
            if (genes[i] != null)
            {
                //Debug.Log(" Caracteristica Gen: " + genes[i].Characteristics.Contains(decorationCharacteristic));
                if (genes[i].Characteristics.Contains(decorationCharacteristic) || genes[i].Characteristics.Contains(decoration2Characteristic))
                    flag = 0;

                foreach (var spawn in spawnersPos)
                {
                    //Calcular distancia
                    var dist = FlatDistance(i, spawn, chrom);

                    //Comparar distancia con un minimo para aumentar el fitness
                    if (dist < dist_1)
                    {
                        flag = 1;
                        break;
                    }

                    if (dist < dist_2)
                    {
                        flag = 2;
                    }
                }

                if      (flag == 0 && genes[i].Characteristics.Contains(decorationCharacteristic)) fitness--;
                else if (flag == 1 && genes[i].Characteristics.Contains(decorationCharacteristic)) fitness++;
                else if (flag == 2 && genes[i].Characteristics.Contains(decorationCharacteristic)) fitness += 0.5f;
                else if (flag == 0 && genes[i].Characteristics.Contains(decoration2Characteristic)) fitness++;
                else if (flag == 1 && genes[i].Characteristics.Contains(decoration2Characteristic)) fitness--;
                else if (flag == 2 && genes[i].Characteristics.Contains(decoration2Characteristic)) fitness -= 0f;
            }
        }

        return fitness / genes.Count;
    }

    public float FlatDistance(int first, int second, BundleTilemapChromosome chrom)
    {
        return (chrom.ToMatrixPosition(first) - chrom.ToMatrixPosition(second)).magnitude;
    }

    public object Clone()
    {
        var e = new DecorationDistribution();
        e.spawnCharacteristic = spawnCharacteristic;
        e.decorationCharacteristic = decorationCharacteristic;
        e.decoration2Characteristic = decoration2Characteristic;
        return e;
    }
}
