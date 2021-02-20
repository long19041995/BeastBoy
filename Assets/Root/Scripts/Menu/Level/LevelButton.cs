using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Sprite spriteLocked;
    [SerializeField] Sprite spriteUnLocked;
    [SerializeField] Sprite spriteCurrent;
    [SerializeField] float currentScale = 1.2f;
    int index = 0;
    int indexLevel = 0;
    int indexPassed = 0;

    public void SetIndex(int index)
    {
        this.index = index;
        indexLevel = DataController.Instance.IndexLevel;
        indexPassed = DataController.Instance.IndexPassed;
    }

    public void Init()
    {
        SetSprite();
        SetText();
    }

    private void SetSprite()
    {
        Sprite sprite = spriteLocked;

        if (index == indexLevel)
        {
            sprite = spriteCurrent;
            transform.localScale = new Vector3(currentScale, currentScale, 1f);
        }
        else if (index <= indexPassed || DataController.Instance.IsBeta)
        {
            sprite = spriteUnLocked;
        }

        Image image = GetComponent<Image>();
        image.sprite = sprite;
    }

    private void SetText()
    {
        Text text = GetComponentInChildren<Text>();
        if (index <= indexPassed || DataController.Instance.IsBeta)
        {
            text.text = (index + 1).ToString();
        }
        else
        {
            text.text = "";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (index <= indexPassed || DataController.Instance.IsBeta)
        {
            DataController.Instance.IndexLevel = index;
            DataController.Instance.IndexWave = 0;
            SceneManager.LoadScene("Game");
        }
    }
}
