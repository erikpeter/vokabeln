using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Correctness
{
    Exact,
    Close,
    Wrong
}

public class VocabularyTest
{
    private List<vocable> vocabulary;
    private int current_vocab = 0;
    private const float decay = 0.5f;

    public bool CheckCapitals{get; set;}

    private static void Swap<T>(ref T arg1, ref T arg2)
    {
        T temp = arg1;
        arg1 = arg2;
        arg2 = temp;
    }

    public static int StringDistance(string source1, string source2) //O(n*m)
    {
        var source1Length = source1.Length;
        var source2Length = source2.Length;

        var matrix = new int[source1Length + 1, source2Length + 1];

        // First calculation, if one entry is empty return full length
        if (source1Length == 0)
            return source2Length;

        if (source2Length == 0)
            return source1Length;

        // Initialization of matrix with row size source1Length and columns size source2Length
        for (var i = 0; i <= source1Length; matrix[i, 0] = i++) { }
        for (var j = 0; j <= source2Length; matrix[0, j] = j++) { }

        // Calculate rows and collumns distances
        for (var i = 1; i <= source1Length; i++)
        {
            for (var j = 1; j <= source2Length; j++)
            {
                var cost = (source2[j - 1] == source1[i - 1]) ? 0 : 1;

                matrix[i, j] = Mathf.Min(
                    Mathf.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                    matrix[i - 1, j - 1] + cost);
            }
        }
        // return result
        return matrix[source1Length, source2Length];
    }


    public VocabularyTest()
    {
        vocabulary = new List<vocable>();
        vocabulary.Add(new vocable("Test", "Test"));
        vocabulary.Add(new vocable("Lösung", "Solution"));
        CheckCapitals = false;
    }

    public string GetPrompt()
    {
        return vocabulary[current_vocab].Prompt;
    }

    public string GetSolution()
    {
        return vocabulary[current_vocab].Solution;
    }

    public Correctness TestIfCorrect(string input)
    {
        string comp_solution = GetSolution();
        if (!CheckCapitals)
        {
            input = input.ToLower();
            comp_solution = comp_solution.ToLower();
        }

        bool exactly_correct = input == comp_solution;
        int new_weight = vocabulary[current_vocab].Weight;
        Correctness output;
        if (exactly_correct)
        {
            new_weight = Mathf.Max(1, Mathf.FloorToInt(decay * new_weight));
            output = Correctness.Exact;
        }
        else
        { 
            int threshold = 1 + Mathf.FloorToInt(0.1666f * (float)vocabulary[current_vocab].Solution.Length);
            int distance = StringDistance(input, comp_solution);
            Debug.Log(distance);
            if (distance <= threshold)
            {
                output = Correctness.Close;
                new_weight = Mathf.Max(2, new_weight);
            }
            else
            {
                new_weight++;
                output = Correctness.Wrong;
            }
            
        }
        vocabulary[current_vocab].Weight = new_weight;
        return output;
    }

    public string GetStats()
    {
        string output_string = "";
        for (int i = 0; i < vocabulary.Count; i++)
        {
            output_string += vocabulary[i].Prompt + "  " + vocabulary[i].Weight.ToString() + "\n";
        }

        return output_string;
    }

    public void Select_Random()
    {
        int sum_weight = 0;
        for (int i = 0; i < vocabulary.Count; i++)
        {
            sum_weight += vocabulary[i].Weight;
        }

        int selection = Random.Range(1, sum_weight + 1);
        
        int index_select = 0; 
        sum_weight = 0;

        for (int i = 0; i < vocabulary.Count; i++)
        {
            sum_weight += vocabulary[i].Weight;
            if (sum_weight >= selection) break;
            index_select++;
        }
        current_vocab = index_select;
    }

}
