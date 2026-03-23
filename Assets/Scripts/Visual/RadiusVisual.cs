using UnityEngine;
using UnityEngine.UI;

public class RadiusVisual : MonoBehaviour
{
    private SpriteRenderer _image;
    private void Awake()
    {
        _image = GetComponent<SpriteRenderer>();
        _image.enabled = false;
        Debug.Log($"Visualize Radius: Image={_image}");
    }
    public void VisualizeRadius(bool isActive)
    {
        if (isActive)
        {
            _image.enabled = true;
            _image.transform.localScale = Vector3.one  * 2;
        }
        else
        {
            _image.enabled = false;
        }
    }
}