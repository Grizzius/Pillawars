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
    public AudioClip[] footsteps;
    public AudioSource audioPrefab;

    // Start is called before the first frame update
    void Start()
    {
        InitColor(colorPlayer);
    }

    public void PlayFootStep()
    {
        StartCoroutine(PlayFootStepRoutine());
    }

    public IEnumerator PlayFootStepRoutine()
    {
        AudioClip clip = footsteps[Random.Range(0, footsteps.Length)];
        var instance = Instantiate(audioPrefab, transform.position, Quaternion.identity, transform);
        yield return new WaitForEndOfFrame();
        instance.enabled = true;
        instance.clip = clip;
        instance.Play();
        yield return new WaitForSeconds(clip.length);
        Destroy(instance.gameObject);
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
