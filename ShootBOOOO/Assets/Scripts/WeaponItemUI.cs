using UnityEngine;
using UnityEngine.UI;

public class WeaponItemUI : MonoBehaviour
{
    public bool isSelect = false;
    public Image selectImage;
    public Image weaponImage;
    public Text ammoText;
    public Color normalColor;
    public Color warningColor;
    public Color activeColor;
    public Color deActiveColor;

    public void ActivateItem(bool _access)
    {
        if (_access)
            weaponImage.color = activeColor;
        else
            weaponImage.color = deActiveColor;
    }

    public void IsSelect(bool _select)
    {
        isSelect = _select;
        selectImage.enabled = _select;
    }

    public void AmmoText(string _text)
    {
        ammoText.text = _text;
    }

    public void AmmoText(string _text, bool _isWarning)
    {
        if (_isWarning)
            ammoText.color = warningColor;
        else
            ammoText.color = normalColor;

        ammoText.text = _text;
    }

}
