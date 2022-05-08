using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Button beginMovementButton;

    [SerializeField]
    private Button beginRandomMovementButton;

    [SerializeField]
    private Button exitApplicationButton;

    [SerializeField]
    private MoveObjectScript moveScript;

    [SerializeField]
    private RandomMoveObjectScript randomMoveScript;

    [SerializeField]
    private Animator moveAnimator;

    [SerializeField]
    private Animator randomMoveAnimator;

    [SerializeField]
    private TimerScript timer;

    [SerializeField]
    private ApplicationCloseScript exitScript;

    private static readonly int startMoveTrigger = Animator.StringToHash("MovementStart");
    private static readonly int endMoveTrigger = Animator.StringToHash("MovementEnd");


    private void Awake()
    {
        moveScript.Data = DownloadFileScript.GetTrajectoryData();

        beginMovementButton.onClick.AddListener(moveScript.StartMovement);
        beginMovementButton.onClick.AddListener(() => moveAnimator.SetTrigger(startMoveTrigger));
        beginMovementButton.onClick.AddListener(() =>
        {
            timer.CurrentLap.gameObject.SetActive(true);
            timer.StartTimer();
        });

        moveScript.OnMovementEnd.AddListener(() => moveAnimator.SetTrigger(endMoveTrigger));
        moveScript.OnLoopFinished.AddListener(() =>
        {
            timer.LastLap.gameObject.SetActive(true);
            timer.Refresh();
        });
        moveScript.OnMovementEnd.AddListener(timer.StopTimer);

        beginRandomMovementButton.onClick.AddListener(randomMoveScript.StartRandomMovement);
        beginRandomMovementButton.onClick.AddListener(() => randomMoveAnimator.SetTrigger(startMoveTrigger));

        randomMoveScript.OnMovementEnd.AddListener(() => randomMoveAnimator.SetTrigger(endMoveTrigger));

        exitApplicationButton.onClick.AddListener(exitScript.Exit);
    }
}
