using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class NeuralNetwork
{
    // FIELDS
    public Layer[] layers;

    // PROPERTIES
    public NeuralNetwork(Layer[] layers)
    {
        this.layers = layers;
    }

    public void EvolveNetwork(float weightChange, float biasChange, float evolutionChance)
    {
        foreach (Layer layer in layers)
        {
            layer.EvolveWeightsAndBiases(weightChange, biasChange, evolutionChance);
        }
    }
    public Node[] GetOutputs()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            if (i > 0)
            {
                layers[i].CalculateNodes(layers[i - 1]);
            }
        }

        return layers[layers.Length - 1].nodes;
    }
    public void SetInputs(float[] inputs)
    {
        for (int node = 0; node < layers[0].nodes.Length; node++)
        {
            layers[0].nodes[node].value = inputs[node];
        }
    }

    public NeuralNetwork Clone()
    {
        Layer[] newLayers = new Layer[layers.Length];
        for (int i = 0; i < newLayers.Length; i++)
        {
            newLayers[i] = new Layer(layers[i].nodes);
        }
        return new NeuralNetwork(newLayers);
    }
}
