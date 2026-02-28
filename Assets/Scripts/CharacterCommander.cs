using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//attach to player
//receive commands and decides when to execute them
public class CharacterCommander : MonoBehaviour
{
    //Queue for this turn (Not executed YET)
    private List<ICommand> turnQueue = new List<ICommand>();
    //manages history for undo/redo
    private CommandHandler commandHandler = new CommandHandler();
    //Delay for every step
    [SerializeField] private float stepDelay = 1.0f;
    
    //stores command for later execution (turnBased)
    public void EnqueueCommand(ICommand command)
    {
        turnQueue.Add(command);
    }

    public void ExecuteTurn()
    {
        StartCoroutine(ExecuteTurnRoutine());
    }

    IEnumerator ExecuteTurnRoutine()
    {
        // Add input restriction while moving
        //execute each queued command in order
        foreach (ICommand command in turnQueue)
        {
            commandHandler.AddAndExecute(command);
            yield return new WaitForSeconds(stepDelay);
        }
        
        turnQueue.Clear();
    }

    public void Undo()
    {
        commandHandler.Undo();
    }
    
    public void Redo()
    {
        commandHandler.Redo();
    }
}