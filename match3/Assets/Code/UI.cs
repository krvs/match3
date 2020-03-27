using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void SetLeftTurns(int turnsLeft)
    {
        _text.text = turnsLeft.ToString();
    }
}
