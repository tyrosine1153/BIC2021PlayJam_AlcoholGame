using System.Linq;
using UnityEngine;

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
}
