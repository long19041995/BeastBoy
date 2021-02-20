using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentScrollView : MonoBehaviour
{
    [SerializeField] GameObject levelButton;
    [SerializeField] GameObject levelPanel;
    [SerializeField] GameObject levelList;
    [SerializeField] GameObject topBar;
    [SerializeField] float numberPerRow = 3f;
    [SerializeField] float spacing = 50f;
    List<GameObject> listButton = new List<GameObject>();


    private void Start()
    {
        StartCoroutine("PreparePlayLevel");
    }

    private IEnumerator PreparePlayLevel()
    {
        yield return new WaitForEndOfFrame();

        int count = DataController.Instance.CurrentMapData.listLevel.Count;

        var rectLevel = levelPanel.GetComponent<RectTransform>().rect;
        var widthCellSize = rectLevel.width / 3;
        var heightCellSize = widthCellSize;

        var gridLayoutGroup = levelPanel.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.cellSize = new Vector2(widthCellSize - spacing * 2, heightCellSize - spacing * 2);
        gridLayoutGroup.spacing = new Vector2(spacing, spacing * 2);

        for (int i = 0; i < count; i++)
        {
            GameObject buttonInstance = Instantiate(levelButton, levelPanel.transform);
            LevelButton button = buttonInstance.GetComponent<LevelButton>();
            button.SetIndex(i);
            button.Init();
            listButton.Add(buttonInstance);
        }

        yield return new WaitForEndOfFrame();

        var heightTopBar = topBar.GetComponent<RectTransform>().rect.height;
        var numberRow = Mathf.Ceil(count / numberPerRow);

        // set sizeDelta for content
        var sizeDelta = GetComponent<RectTransform>().sizeDelta;
        GetComponent<RectTransform>().sizeDelta = new Vector2(sizeDelta.x, heightTopBar + heightCellSize * numberRow);
    }
}
