using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Button beginMovementButton;

    [SerializeField]
    private MoveObjectScript moveScript;

    private void Awake()
    {
        moveScript.Data = DownloadFileScript.GetTrajectoryData();
        beginMovementButton.onClick.AddListener(moveScript.StartMovement);
    }
}
