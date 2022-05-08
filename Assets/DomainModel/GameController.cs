using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Button beginMovementButton;

    [SerializeField]
    private Button beginRandomMovementButton;

    [SerializeField]
    private MoveObjectScript moveScript;

    [SerializeField]
    private RandomMoveObjectScript randomMoveScript;

    [SerializeField]
    private Animator moveAnimator;

    [SerializeField]
    private Animator randomMoveAnimator;

    private static readonly int startMoveTrigger = Animator.StringToHash("MovementStart");
    private static readonly int endMoveTrigger = Animator.StringToHash("MovementEnd");


    private void Awake()
    {
        moveScript.Data = DownloadFileScript.GetTrajectoryData();
        beginMovementButton.onClick.AddListener(moveScript.StartMovement);
        moveScript.OnMovementEnd.AddListener(() => moveAnimator.SetTrigger(endMoveTrigger));
        beginMovementButton.onClick.AddListener(() => moveAnimator.SetTrigger(startMoveTrigger));
        beginRandomMovementButton.onClick.AddListener(randomMoveScript.StartRandomMovement);
        beginRandomMovementButton.onClick.AddListener(() => randomMoveAnimator.SetTrigger(startMoveTrigger));
        randomMoveScript.OnMovementEnd.AddListener(() => randomMoveAnimator.SetTrigger(endMoveTrigger));
    }
}
