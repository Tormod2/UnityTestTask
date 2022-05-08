using DG.Tweening;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RandomMoveObjectScript : MonoBehaviour
{
    [SerializeField]
    private Transform objectTransform;

    [SerializeField]
    private LineRenderer originalLine;

    [SerializeField]
    private LineRenderer bezierLine;

    [SerializeField]
    private float time = 30f;

    public UnityEvent OnMovementEnd;

    private Sequence currentSequence;
    private bool needsReset;

    /// <summary>
    /// Defines movement trajectory for the object based on a randomly generated trajectory and a given time
    /// </summary>
    public void StartRandomMovement()
    {
        if(needsReset)
        {
            currentSequence.Kill();
        }

        var trajectory = PathGeneratorScript.GenerateRandomPoints(35);
        var bezierTrajectory = BezierCurveScript.MakeBezierCurve(0.08f,trajectory);

        DrawLineScript.DrawLine(trajectory, originalLine);
        DrawLineScript.DrawLine(bezierTrajectory, bezierLine);

        //Setting the starting position of the ball to the first point of trajectory
        objectTransform.position = bezierTrajectory[0];

        currentSequence = PathGeneratorScript.GenerateSequence(bezierTrajectory, objectTransform, time);

        currentSequence.OnComplete(() => {
            OnMovementEnd?.Invoke();
        });

        DOTween.Play(currentSequence);
        needsReset = true;
    }
}
