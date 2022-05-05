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
    private UnityEvent loopends;

    [SerializeField]
    private UnityEvent endOfTheLine;

    private PathData data;

    //
    private void Awake()
    {
        data = DownloadFileScript.GetTrajectoryData();
        StartMovement();
    }

    /// <summary>
    /// Defines movement trajectory for the object based on a loaded data
    /// </summary>
    public void StartMovement()
    {
        var startPoint = data.Trajectory[0];
        startPoint.y += objectTransform.position.y;

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
                loopends?.Invoke();
                sequence.Restart();
            });
        }
        else
        {
            sequence.OnComplete(() => {
                endOfTheLine?.Invoke();
            });
        }

        DOTween.Play(sequence);
    }
}
