using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToastController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI popupDesc;
    [SerializeField] string textBase;
    [SerializeField] float timer = 2f;
    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Toast();
    }

    public void Toast()
    {
        gameObject.SetActive(true);
        popupDesc.text = textBase;
        StartCoroutine(HidePopup());
    }

    IEnumerator HidePopup()
    {
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
    }

    public void SetText(string newText, float time)
    {
        timer = time;
        textBase = newText;
    }
}
