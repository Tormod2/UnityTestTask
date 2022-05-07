using DG.Tweening;
using System.Linq;
using UnityEngine;

public class RandomMoveObjectScript : MonoBehaviour
{
    [SerializeField]
    private Transform objectTransform;

    [SerializeField]
    private LineRenderer originalLine;

    [SerializeField]
    private float time = 30f;

    /// <summary>
    /// Defines movement trajectory for the object based on a randomly generated trajectory and a given time
    /// </summary>
    public void StartRandomMovement()
    {
        var dots = PathGeneratorScript.GeneratePoints(35);
        var trajectory = dots.Select(dot => new Vector3(dot.x, 2f, dot.y)).ToList();

        DrawLineScript.DrawLine(trajectory, originalLine);

        //Setting the starting position of the ball to the first point of trajectory
        objectTransform.position = trajectory[0];

        DOTween.Play(PathGeneratorScript.GenerateSequence(trajectory, objectTransform, time));
    }
}
