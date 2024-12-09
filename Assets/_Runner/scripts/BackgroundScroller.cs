using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 0.1f;
    public bool scrollHorizontally;
    Material material;
    private float offset;
    string maintext = "_MainTex";

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    private void FixedUpdate()
    {
        if (scrollHorizontally)
        {
            ScrollHorizontally();
        }
        else
        {
            ScrollVertically();
        }
    }

    private void ScrollVertically()
    {
        offset += Time.deltaTime * scrollSpeed;
        material.SetTextureOffset(maintext, Vector2.up * offset);
    }

    private void ScrollHorizontally()
    {
        offset += Time.deltaTime * scrollSpeed;
        material.SetTextureOffset(maintext, Vector2.right * offset);
    }
}
