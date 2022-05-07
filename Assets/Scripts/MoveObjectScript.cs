using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class MoveObjectScript : MonoBehaviour
{
    [SerializeField]
    private Transform objectTransform;

    [SerializeField]
    private LineRenderer line;

    [SerializeField]
    private LineRenderer bezierLine;

    public UnityEvent OnLoopFinished;

    public UnityEvent OnMovementEnd;

    private PathData data;
    private bool needsReset = false;
    private Sequence currentSequence;

    public PathData Data
    {
        get => data;
        set => data = value;
    }

    /// <summary>
    ///
    /// </summary>
    public void StartMovement()
    {
        if (needsReset)
        {
            Data = DownloadFileScript.GetTrajectoryData();
            currentSequence.Kill();
        }

        var startPoint = Data.Trajectory[0];

        //Setting the starting position of the object to the first point of trajectory
        objectTransform.position = startPoint;

        if(Data.Loop)
        {
            Data.Trajectory.Add(startPoint);
        }

        var bezierTrajectory = BezierCurveScript.MakeBezierCurve(0.08f, Data.Trajectory);
        DrawLineScript.DrawLine(Data.Trajectory, line);
        DrawLineScript.DrawLine(bezierTrajectory, bezierLine);

        currentSequence = PathGeneratorScript.GenerateSequence(bezierTrajectory, objectTransform, Data.Time);
        //currentSequence = PathGeneratorScript.GenerateSequence(Data.Trajectory, objectTransform, Data.Time);

        if (Data.Loop)
        {
            currentSequence.OnComplete(() => {
                OnLoopFinished?.Invoke();
                currentSequence.Restart();
            });
        }
        else
        {
            currentSequence.OnComplete(() => {
                OnMovementEnd?.Invoke();
            });
        }

        DOTween.Play(currentSequence);
        needsReset = true;
    }
}
