using UnityEngine;
using TMPro;
public class UIPresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ChanceNumbersTxt;
    private WheelSpinning wheelSpinning;
    void Start()
    {
        wheelSpinning = GetComponent<WheelSpinning>();
        wheelSpinning.OnSpinsRemainChange += UpdateChanceNumber;
    }
    private void UpdateChanceNumber()
    {
        ChanceNumbersTxt.text = $"{wheelSpinning.SpinsRemain} CHANCES";
    }
}
