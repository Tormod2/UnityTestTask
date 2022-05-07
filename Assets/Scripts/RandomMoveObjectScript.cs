using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class RandomMoveObjectScript : MonoBehaviour
{
    [SerializeField]
    private Transform objectTransform;

    [SerializeField]
    private LineRenderer originalLine;

    [SerializeField]
    private LineRenderer bezierLine;

    [SerializeField]
    private float time = 15f;

    /// <summary>
    /// Defines movement trajectory for the object based on a randomly generated trajectory and a given time
    /// </summary>
    public void StartRandomMovement()
    {
        var trajectory = PathGeneratorScript.GenerateRandomPath(-100f, 100f, 2, -100f, 100f, 35);
        var bezierTrajectory = BezierCurveScript.MakeBezierCurve(0.08f, trajectory);
        
        DrawLineScript.DrawLine(trajectory, originalLine);
        DrawLineScript.DrawLine(bezierTrajectory, bezierLine);

        //Setting the starting position of the ball to the first point of trajectory
        objectTransform.position = bezierTrajectory[0];
        
        
        DOTween.Play(PathGeneratorScript.GenerateSequence(bezierTrajectory, objectTransform, time));
    }
}
