using UnityEngine;
using UnityEngine.InputSystem;  // For the new Input System

public class Highlighter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private MaterialPropertyBlock propertyBlock;
    private int outlineEnabledID;
    private bool isInProximity = true;  // By default, allow outline
    private Material material;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        propertyBlock = new MaterialPropertyBlock();
        outlineEnabledID = Shader.PropertyToID("_OutlineEnabled");
        // Get the material instance from the SpriteRenderer
        material = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        // Only check mouse hover if we're within proximity
        if (isInProximity && IsMouseOverObject())
        {
            EnableOutline();
            Debug.Log("Mouse is over the object: " + gameObject.name);  // Log when the mouse is over the object
        }
        else
        {
            DisableOutline();
            Debug.Log("Mouse is NOT over the object: " + gameObject.name);  // Log when the mouse is not over the object
        }
    }

    public void SetProximity(bool proximity)
    {
        isInProximity = proximity;
    }

    private bool IsMouseOverObject()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());  // Get mouse position
        Bounds bounds = spriteRenderer.bounds;  // Get the sprite's bounds

        // Check if mouse is inside the sprite's bounds
        return bounds.Contains(mousePosition);
    }

    public void EnableOutline()
    {
        Debug.Log("Enabling outline for: " + gameObject.name);
        // Enable the outline keyword in the material
        material.EnableKeyword("OUTLINE_ON");
    }

    public void DisableOutline()
    {
        Debug.Log("Disabling outline for: " + gameObject.name);
        // Disable the outline keyword in the material
        material.DisableKeyword("OUTLINE_ON");
    }

}
