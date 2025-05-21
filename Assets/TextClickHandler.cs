using Mirror;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextClickHandler : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI textMeshPro; //тест
    public GameObject playerObject;

    void Start()
    {

        if (textMeshPro == null)
            textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (NetworkClient.localPlayer == null) return;
        playerObject = NetworkClient.localPlayer.gameObject;

        Vector2 localMousePosition = eventData.position;
        Camera camera = eventData.pressEventCamera;

        int linkIndex = TMP_TextUtilities.FindIntersectingLine(textMeshPro, localMousePosition, camera);

        if (linkIndex != -1 && linkIndex < textMeshPro.textInfo.lineCount)
        {
            string lineText = GetLineText(linkIndex);
            Debug.Log($"Нажата строка: {lineText}");

            string actionKey = ParseKeyFromLine(lineText);
            if (!string.IsNullOrEmpty(actionKey))
            {
                SimulateKeyPress(actionKey);
            }
        }
    }

    private string GetLineText(int lineIndex)
    {
        var textInfo = textMeshPro.textInfo;
        int startIndex = textInfo.lineInfo[lineIndex].firstCharacterIndex;
        int length = textInfo.lineInfo[lineIndex].characterCount;

        return textMeshPro.text.Substring(startIndex, length).Trim();
    }

    private string ParseKeyFromLine(string line)
    {
        // Ожидаем формат: "Cursor - F1" или "Crystal - Q"
        var parts = line.Split('-');
        if (parts.Length == 2)
        {
            return parts[1].Trim().ToUpper();
        }
        return null;
    }

    private void SimulateKeyPress(string key)
    {
        switch (key)
        {
            case "F1":
                playerObject.GetComponent<CameraController>().SwitchCursorMode();
                Debug.Log("Cursor (F1) activated!");
                break;
            case "F2":
                playerObject.GetComponent<PlayerAdmin>().SwitchNoclip();
                Debug.Log("Noclip (F2) activated!");
                break;
            case "Q":
                playerObject.GetComponent<ThrowCrystal>().CmdThrow();
                Debug.Log("Crystal (Q) activated!");
                break;
            case "E":
                playerObject.GetComponent<BlinkAbility>().Blink();
                Debug.Log("Blink (E) activated!");
                break;
            case "R":
                playerObject.GetComponent<MoveCamera>().ToggleCameraMode();
                Debug.Log("3 Camera (R) activated!");
                break;
            case "T":
                playerObject.GetComponent<SpawnTank>().CmdSpawnTank();
                Debug.Log("Tank (T) activated!");
                break;
            case "G":
                playerObject.GetComponent<Urinate>().CmdUrinate();
                Debug.Log("Urinate (G) activated!");
                break;
            case "H":
                playerObject.GetComponent<GoAway>().CMDSayGoAway();
                Debug.Log("Go away (H) activated!");
                break;
            default:
                Debug.Log($"Действие для {key} не назначено.");
                break;
        }
    }
}
