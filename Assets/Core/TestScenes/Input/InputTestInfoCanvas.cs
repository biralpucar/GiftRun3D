using System.Text;
using UnityEngine;

public class InputTestInfoCanvas : MonoBehaviour
{
    public TMPro.TextMeshProUGUI infoText;

    private void Start()
    {
        InputManager.instance.TouchStartEvent += PrintData;
        InputManager.instance.TouchContinueEvent += PrintData;
        InputManager.instance.TouchEndEvent += PrintData;

        PrintData(InputManager.instance.touchData);
    }

    void PrintData(InputManager.TouchData data)
    {
        StringBuilder builder = new StringBuilder();

        bool isLevelActive = GameManager.instance.isLevelActive;
        builder.AppendLine(nameof(isLevelActive) + ": " + isLevelActive.ToString());
        builder.AppendLine(string.Empty);

        InputManager.TouchState touchState = InputManager.instance.touchState;
        builder.Append(nameof(touchState) + ":\n    " + touchState.ToString());
        builder.AppendLine(string.Empty);

        Vector3 fingerScreenPos = InputManager.instance.fingerScreenPos;
        builder.Append(nameof(fingerScreenPos) + ":\n    " + fingerScreenPos.ToString("F0"));
        builder.AppendLine(string.Empty);


        builder.AppendLine(nameof(data.initialPos) + ":\n    " + data.initialPos.ToString("F3"));
        builder.AppendLine(nameof(data.currentPos) + ":\n    " + data.currentPos.ToString("F3"));
        builder.AppendLine(nameof(data.absoluteDelta) + ":\n    " + data.absoluteDelta.ToString("F3"));
        builder.AppendLine(nameof(data.frameDelta) + ":\n    " + data.frameDelta.ToString("F3"));

        infoText.text = builder.ToString();
    }
}
