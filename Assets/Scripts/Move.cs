using UnityEngine;

public class Move : ICommand
{
    //the object will be move in
    private Transform target;
    //Direction  ro move in (Vector3.forward)
    private Vector3 direction;
    private float distance;

    //Constructor. everything the command needs is inside itself
    public Move(Transform target, Vector3 direction, float distance)
    {
        this.target = target; //store recievert reference
        this.direction = direction; //store movement direction
        this.distance = distance; //store movement distance
    }
    public void Execute()
    {
        //throw new System.NotImplementedException();
        target.position += direction * distance;
    }

    public void Undo()
    {
        //throw new System.NotImplementedException();
        target.position -= direction * distance;
    }


}