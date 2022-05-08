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
    private float time = 30f;

    public UnityEvent OnMovementEnd;

    private Sequence currentSequence;

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

        currentSequence = PathGeneratorScript.GenerateSequence(trajectory, objectTransform, time);

        currentSequence.OnComplete(() => {
            OnMovementEnd?.Invoke();
        });

        DOTween.Play(currentSequence);
    }
}
