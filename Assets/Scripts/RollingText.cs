using UnityEngine;
using UnityEngine.UI;

public class CreditsRoller : MonoBehaviour
{
    public RectTransform creditsText;  // Assign the Text RectTransform
    public float scrollSpeed = 20f;    // Speed of the scroll
    public float startDelay = 2f;      // Delay before the credits start rolling
    public float stopAt = -100f;       // Y position to stop the credits

    private Vector3 startPosition;     // Store the start position
    private bool rolling = false;

    void Start()
    {
        // Store the initial position of the credits text
        startPosition = creditsText.localPosition;

        // Start rolling the credits after a delay
        Invoke("StartRolling", startDelay);
    }

    void Update()
    {
        if (rolling)
        {
            // Move the credits text upwards
            creditsText.localPosition += Vector3.up * scrollSpeed * Time.deltaTime;

            // Stop rolling when the text reaches a certain position
            if (creditsText.localPosition.y >= stopAt)
            {
                rolling = false;
            }
        }
    }

    void StartRolling()
    {
        // Begin rolling the credits
        rolling = true;
    }

    public void RestartCredits()
    {
        // Reset the credits to the start position and begin rolling again
        creditsText.localPosition = startPosition;
        rolling = true;
    }
}