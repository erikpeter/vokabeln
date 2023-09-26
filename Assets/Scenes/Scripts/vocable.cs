using System.Collections;
using System.Collections.Generic;


public class vocable
{
    public string Prompt { get; set; }
    public string Solution { get; set; }
    public int Weight { get; set; }

    public vocable(string in_prompt, string in_sol)
    {
        Prompt = in_prompt;
        Solution = in_sol;
        Weight = 1;
    }
}
