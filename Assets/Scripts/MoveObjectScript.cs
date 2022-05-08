using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class MoveObjectScript : MonoBehaviour
{
    [SerializeField]
    private Transform objectTransform;

    [SerializeField]
    private LineRenderer line;

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
    ///Method that generates a path to follow.
    /// </summary>
    public void StartMovement()
    {
        if (needsReset)
        {
            Data = DownloadFileScript.GetTrajectoryData();
            currentSequence.Kill();
        }

        var startPoint = Data.Trajectory[0];

        //Setting the starting position of the object to the first point of trajectory.
        objectTransform.position = startPoint;

        if(Data.Loop)
        {
            Data.Trajectory.Add(startPoint);
        }

        DrawLineScript.DrawLine(Data.Trajectory, line);

        currentSequence = PathGeneratorScript.GenerateSequence(Data.Trajectory, objectTransform, Data.Time);

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
