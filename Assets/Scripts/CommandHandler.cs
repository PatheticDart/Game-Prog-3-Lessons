using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

//Manager for our commands
//it decides how commands are stored and replayed (DO/UNDO/REDO)
//We will do these by making list of <commands>
//Index = 0 1 2 3 (The move)
public class CommandHandler
{
    private List<ICommand> history = new List<ICommand>();

    public GameObject[] obj;
    //if index == history.Count => All commands are applied
    private int index = 0;

    public void AddAndExecute(ICommand command)
    {
        //if we undone some commands, index is not at end
        //adding new command means "redo future"
        if (index < history.Count)
        {
            //Remove commands from index to end
            history.RemoveRange(index, history.Count- index);
        }
        //add new command
        history.Add(command);
        //execute it immediately
        command.Execute();
        index++;//move index forward
    }

    public void Undo()
    {
        if (index <= 0) return;
        index--;
        history[index].Undo();
    }

    public void Redo() 
    {
        if (index >= history.Count) return;
        history[index].Execute();
        index++;
    }

    public void Clear()
    {
        history.Clear();
        index = 0;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}