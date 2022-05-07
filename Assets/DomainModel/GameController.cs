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

    private void Awake()
    {
        moveScript.Data = DownloadFileScript.GetTrajectoryData();
        beginMovementButton.onClick.AddListener(moveScript.StartMovement);
        beginRandomMovementButton.onClick.AddListener(randomMoveScript.StartRandomMovement);
    }
}
