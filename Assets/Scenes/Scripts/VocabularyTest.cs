using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VocabularyTest
{
    private List<vocable> vocabulary;
    private int current_vocab = 0;
    private const float decay = 0.5f;

    public bool CheckCapitals{get; set;}

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

    private string GetSolution()
    {
        return vocabulary[current_vocab].Solution;
    }

    public bool TestIfCorrect(string input)
    {
        string comp_solution = GetSolution();
        if (!CheckCapitals)
        {
            input = input.ToLower();
            comp_solution = comp_solution.ToLower();
        }
        bool correct = input == comp_solution;
        int new_weight = vocabulary[current_vocab].Weight;
        Debug.Log(new_weight);
        if (correct)
        {
            new_weight = Mathf.Max(1, Mathf.FloorToInt(decay * new_weight));
        }
        else
        {
            new_weight++;
        }
        vocabulary[current_vocab].Weight = new_weight;
        return correct;

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
