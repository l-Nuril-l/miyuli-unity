using Mirror;
using UnityEngine;

public class BlinkAbility : NetworkBehaviour
{
    public float maxBlinkDistance = 10f;
    public KeyCode interactKey = KeyCode.E;
    public float minHeightAboveGround = 1f; // Ìèíèìàëüíàÿ âûñîòà íàä çåìëåé

    private Transform cameraPos;
    private Transform orientation;

    private void Start()
    {
        cameraPos = transform.Find("CameraPos");
        orientation = transform.Find("Orientation");
    }

    private void Update()
    {
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown(interactKey))
        {
            Blink();
        }
    }

    public void Blink()
    {
        // Ïîëó÷àåì ïîçèöèþ èãðîêà
        Vector3 playerPosition = cameraPos.position;

        // Ïîëó÷àåì íàïðàâëåíèå âçãëÿäà èãðîêà
        Vector3 playerForward = orientation.forward;

        // Ðàññ÷èòûâàåì êîíå÷íóþ ïîçèöèþ äëÿ áëèíêà
        Vector3 blinkPosition = playerPosition + playerForward * maxBlinkDistance;

        // Ïðîâîäèì ëó÷ îò òåêóùåé ïîçèöèè èãðîêà â íàïðàâëåíèè âçãëÿäà
        RaycastHit hit;
        if (Physics.Raycast(playerPosition, playerForward, out hit, maxBlinkDistance))
        {
            // Òåëåïîðòèðóåì èãðîêà è êàìåðó ê òî÷êå
            blinkPosition = hit.point;
        }

        blinkPosition.y += 0.01f;
        if (Physics.Raycast(blinkPosition, Vector3.down, out hit, 1))
        {
            // Ïîäíèìàåì êîíå÷íóþ òî÷êó íà ìèíèìàëüíóþ âûñîòó íàä çåìëåé
            blinkPosition.y += 1 - (blinkPosition.y - hit.point.y);
        }

        transform.position = blinkPosition;
    }
}