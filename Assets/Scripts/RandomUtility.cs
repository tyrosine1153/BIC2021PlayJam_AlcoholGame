using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomUtility : MonoBehaviour
{
    private float[] probabilities = {20, 50, 30};

    public static int CalculateProbability(float[] probs)
    {
        var total = probs.Sum();
        var randomPoint = Random.value * total;

        for (var i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        } return probs.Length - 1;
    }

    public static bool CalculateProbability(float prob)
    {
        if (prob >= 100) return true;
        if (prob <= 0) return false;

        return CalculateProbability(new[] {prob, 100 - prob}) == 0;
    }

    public static Tuple<string, int> CreateMathQuestion()
    {
        var a = Random.Range(0, 101);
        var b = Random.Range(0, 101);
        var calculationSign = Random.Range(0, 3);

        var formula = "";
        var result = 0;
        switch (calculationSign)
        {
            case 0:
                result = a + b;
                formula = $"{a} + {b} =";
                break;
            case 1:
                result = a - b;
                formula = $"{a} - {b} =";
                break;
            case 2:
                result = a * b;
                formula = $"{a} * {b} =";
                break;
        }

        return new Tuple<string, int>(formula, result);
    }
}
