using UnityEngine;

public class ProximityLogic : MonoBehaviour
{
    public float interactionDistance = 0.5f;  // Distance threshold to interact
    private Transform playerTransform;
    private ChestController chestController;
    private Highlighter highlighter;  // Reference to the Highlighter component

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        chestController = GetComponent<ChestController>();
        highlighter = GetComponent<Highlighter>();
    }

    void Update()
    {
        // Check distance between player and chest
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // Notify the highlighter whether the player is within proximity
        if (distanceToPlayer < interactionDistance)
        {
            highlighter.SetProximity(true);  // Player is close enough, allow highlight
        }
        else
        {
            highlighter.SetProximity(false);  // Player is too far away, block highlight
        }
    }
}
