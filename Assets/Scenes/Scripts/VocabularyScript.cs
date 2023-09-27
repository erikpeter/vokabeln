using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class VocabularyScript:MonoBehaviour
{

    private VocabularyTest test;
    private const float std_delay = 0.25f;
    public TextMeshProUGUI prompt_text; 
    public TextMeshProUGUI feedback;
    public TextMeshProUGUI stats;
    public TMP_InputField input_field;
    private bool stat_mode;

    bool current_input;
    float input_delay;
    // Start is called before the first frame update
 
    void handle_input(string msg)
    {
        current_input = false;
        string entered = input_field.text;
        input_field.interactable = false;
      

        Correctness correct = test.TestIfCorrect(entered);

        

        if (correct == Correctness.Exact)
        {
            feedback.text = "Richtig!";
            feedback.color = Color.green;
        }
        else if (correct == Correctness.Wrong) // case wrong
        {
            feedback.text = "Falsch!";
            feedback.color = Color.red;
        }
        else
        {
            feedback.text = "Fast!";
            feedback.color = Color.yellow;
        }
        feedback.text = feedback.text + " " + test.GetSolution();
        input_delay = std_delay;
    }

    private void GetNext()
    {
        test.Select_Random();
        prompt_text.text = test.GetPrompt();
        feedback.text = "";
        current_input = true;
        input_delay = 0.0f;
        input_field.interactable = true;
        input_field.ActivateInputField();
        input_field.text = "";
    }

    void Start()
    {
        test = new VocabularyTest();
        GetNext();
        input_field.onSubmit.AddListener(handle_input);
        stat_mode = false;
        ShowStats();
    }
    private void ShowStats()
    {
        if (stat_mode)
        {
            stats.text = test.GetStats();
        }
        else
        {
            stats.text = "";
        }
    }
    // Update is called once per frame
    void Update()
    {
        input_delay = input_delay - Time.deltaTime;
        if (input_delay < 0.0f) input_delay = 0.0f;

        if (Input.GetKeyDown(KeyCode.F1) & input_delay == 0.0f)
        {
            stat_mode = !stat_mode;
            input_delay = std_delay;
        }
        else
        {
            if (!current_input) // if no current text field input listen to input here instead
            {
                if (input_delay == 0.0f)
                {
                    if (Input.anyKey) GetNext();               
                }

            }
        }
        ShowStats();
    }
}
