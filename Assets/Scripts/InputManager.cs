using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public Button up, down, left, right;
    public Button finishTurn, undo, redo;
    //Receiver
    public CharacterCommander character;
    public float distance; // how far the player goes

    private void OnEnable()
    {
        up.onClick.AddListener(()=> SendMoveCommand(Vector3.forward));
        down.onClick.AddListener(()=> SendMoveCommand(Vector3.back));
        left.onClick.AddListener(()=> SendMoveCommand(Vector3.left));
        right.onClick.AddListener(()=> SendMoveCommand(Vector3.right));

        finishTurn.onClick.AddListener(()=> character.ExecuteTurn());
        undo.onClick.AddListener(()=> character.Undo());
        redo.onClick.AddListener(()=> character.Redo());
    }

    private void SendMoveCommand(Vector3 direction)
    {
        ICommand move = new Move(character.transform, direction, distance);
        character.EnqueueCommand(move);
    }
}
