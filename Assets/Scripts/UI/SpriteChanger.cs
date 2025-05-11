using UnityEngine;
using UnityEngine.UI;

public class SpriteChanger : MonoBehaviour
{
    public Slider slider;
    public Sprite enabledSprite;
    public Sprite disabledSprite;

    private Image _image;

    void Awake()
    {
        _image = GetComponent<Image>();
        if (slider != null)
            slider.wholeNumbers = true;
    }

    void Start()
    {
        ChangeSprite();
    }

    public void ChangeSprite()
    {
        if (_image == null || slider == null) return;

        _image.sprite = (slider.value == slider.minValue) ? disabledSprite : enabledSprite;
    }
}