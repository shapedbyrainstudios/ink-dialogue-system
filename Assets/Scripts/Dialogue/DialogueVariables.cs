using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System.IO;

public class DialogueVariables
{
    public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }

    public DialogueVariables() 
    {
        // initialize the dictionary with all global variables
        variables = new Dictionary<string, Ink.Runtime.Object>();
        // initialize the variable 'pokemon_name' as a string, 
        // which should match the globals ink file. Use BoolValue, FloatValue, etc.. for different types
        variables.Add("pokemon_name", new Ink.Runtime.StringValue(""));
    }

    public void StartListening(Story story) 
    {
        // it's important that VariablesToStory is before assigning the listener!
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged;
    }

    public void StopListening(Story story) 
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }

    private void VariableChanged(string name, Ink.Runtime.Object value) 
    {
        // only maintain variables that were initialized from the globals ink file
        if (variables.ContainsKey(name)) 
        {
            variables.Remove(name);
            variables.Add(name, value);
        }
    }

    private void VariablesToStory(Story story) 
    {
        foreach(KeyValuePair<string, Ink.Runtime.Object> variable in variables) 
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }

}
