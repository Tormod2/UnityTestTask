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

    public PathData Data
    {
        get => data;
        set => data = value;
    }


    /// <summary>
    /// Defines movement trajectory for the object based on a loaded data
    /// </summary>
    public void StartMovement()
    {
        if (needsReset)
        {
            Data = DownloadFileScript.GetTrajectoryData();
            DOTween.Clear();
        }
        var startPoint = data.Trajectory[0];

        //Setting the starting position of the ball to the first point of trajectory
        objectTransform.position = startPoint;

        if(data.Loop)
        {
            data.Trajectory.Add(startPoint);
        }

        //DrawScript.DrawLine(data.Trajectory, line);
        var sequence = PathGeneratorScript.GenerateSequence(data.Trajectory, objectTransform, data.Time);

        if (data.Loop)
        {
            sequence.OnComplete(() => {
                OnLoopFinished?.Invoke();
                sequence.Restart();
            });
        }
        else
        {
            sequence.OnComplete(() => {
                OnMovementEnd?.Invoke();
            });
        }

        DOTween.Play(sequence);
        needsReset = true;
    }
}
