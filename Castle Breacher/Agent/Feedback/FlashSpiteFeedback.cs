using System.Collections;
using UnityEngine;

public class FlashSpiteFeedback : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private float flashTime = 0.1f; //Time between the flash
    [SerializeField] private Material flashMaterial = null;

    private Shader originalMaterialShader; //For saving the shader of the player

    private void Start()
    {
        originalMaterialShader = spriteRenderer.material.shader; //Saves the shader of the player
    }

    public void FinishFeedback()
    {
        if (spriteRenderer.material.shader != originalMaterialShader)
            spriteRenderer.material.shader = originalMaterialShader;
    }

    /// <summary>
    /// Changes the shader off the player
    /// </summary>
    public void CreateFeedback()
    {
        if (spriteRenderer.material.HasProperty("_MakeSolidColor") == false)
        {
            spriteRenderer.material.shader = flashMaterial.shader;
        }
        //spriteRenderer.material.SetInt("_MakeSolidColor", 1);
        StartCoroutine(WaitBeforeChangingBack());
    }

    /// <summary>
    /// Changes The shader of the player 
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitBeforeChangingBack()
    {
        yield return new WaitForSeconds(flashTime);
        spriteRenderer.material.shader = originalMaterialShader;
        yield return new WaitForSeconds(flashTime);
        spriteRenderer.material.shader = flashMaterial.shader;
        yield return new WaitForSeconds(flashTime);
        spriteRenderer.material.shader = originalMaterialShader;

        FinishFeedback();
    }
}
