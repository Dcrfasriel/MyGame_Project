using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperationUIController : MonoBehaviour
{
    List<Image> Images = new List<Image>();
    List<Text> Texts = new List<Text>();
    private void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Image>() != null)
            {
                Images.Add(transform.GetChild(i).GetComponent<Image>());
            }
            else if(transform.GetChild(i).GetComponent<Text>() != null)
            {
                Texts.Add(transform.GetChild(i).GetComponent<Text>());
            }
        }
    }

    public void SetDisplay(bool value)
    {
        foreach (Image image in Images)
        {
            image.gameObject.SetActive(value);
        }
        foreach(Text text in Texts)
        {
            text.gameObject.SetActive(value);
        }
    }
}
