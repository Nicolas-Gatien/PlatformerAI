using UnityEngine;
using System;

public class Layer
{
    public Node[] nodes;

    public Layer(int numOfNodes)
    {
        nodes = new Node[numOfNodes];
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = InitializeWeightsAndBiases(-1f, 1f, -1f, 1f);
        }
    }

    public Layer(Node[] _nodes)
    {
        Node[] newNodes = new Node[_nodes.Length];
        for (int i = 0; i < newNodes.Length; i++)
        {
            newNodes[i] = new Node(
                _nodes[i].value,
                _nodes[i].weight,
                _nodes[i].bias
                );
        }

        this.nodes = newNodes;
    }

    public void CalculateNodes(Layer previousLayer)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].value = 0;
            foreach (var item in previousLayer.nodes)
            {
                nodes[i].value += ActivationFunction((item.value * item.weight));
            }

            nodes[i].value += nodes[i].bias;
        }
    }

    Node InitializeWeightsAndBiases(float weightMin, float weightMax, float biasMin, float biasMax)
    {
        float weight = UnityEngine.Random.Range(weightMin, weightMax);
        float bias = UnityEngine.Random.Range(biasMin, biasMax);
        return new Node(0, weight, bias);
    }

    public void EvolveWeightsAndBiases(float weightChange, float biasChange, float chanceOfChange)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            if (UnityEngine.Random.Range(0, 100) < chanceOfChange)
            {
                nodes[i].weight += UnityEngine.Random.Range(-weightChange, weightChange);
            }
            if (UnityEngine.Random.Range(0, 100) < chanceOfChange)
            {
                nodes[i].bias += UnityEngine.Random.Range(-biasChange, biasChange);
            }
        }
    }

    double ActivationFunction(double value)
    {
        return Math.Tanh(value);
    }
}
