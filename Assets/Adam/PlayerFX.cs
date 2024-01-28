using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    public Movement playerMovement;
    public Color colorPlayer;
    public SpriteRenderer cursorSpriteRenderer;
    public SpriteRenderer ChargerSpriteRenderer;
    public Transform chargeTransform;

    // Start is called before the first frame update
    void Start()
    {
        InitColor(colorPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        float ratio = playerMovement.GetJumpCharge();
        SetJumpCharge(ratio);
    }

    public void InitColor(Color color)
    {
        colorPlayer = color;
        cursorSpriteRenderer.color = colorPlayer;
        ChargerSpriteRenderer.color = colorPlayer;
    }

    /// <summary>
    /// entre 0 et 1 où 1 correspond à une charge pleine
    /// </summary>
    /// <param name=""></param>
    public void SetJumpCharge(float c)
    {
        chargeTransform.localScale = new Vector3(c, c, c);
    }
}
