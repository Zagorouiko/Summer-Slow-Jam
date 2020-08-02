using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CopyTextToInputField : MonoBehaviour
{
    public GameObject Inputfield;
    private string currentText;
    TextMeshPro text;

    private void Start()
    {
        text = GetComponent<TextMeshPro>();
        currentText = GetComponent<TextMeshPro>().text;
    }

    void FixedUpdate()
    {
        SetCurrentText();        
    }

    void SetCurrentText()
    {
        if (currentText.Length != text.text.Length)
        {
            Inputfield.GetComponent<TMP_InputField>().text = GetComponent<TextMeshPro>().text;
            currentText = text.text;
        }
    }
}
