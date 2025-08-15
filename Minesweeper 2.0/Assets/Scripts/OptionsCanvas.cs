using UnityEngine;
using UnityEngine.UI;

public class OptionsCanvas : MonoBehaviour
{
    [SerializeField] private Toggle _adsOfferingToggle;

    private void OnEnable()
    {
        _adsOfferingToggle.isOn = !ContinueWindow.DontShowAgain;
    }

    public void OnAdOfferingToggle()
    {
        EventBus.OnButtonClick?.Invoke();
        ContinueWindow.DontShowAgain = !_adsOfferingToggle.isOn;
    }
}