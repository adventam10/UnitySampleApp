using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void ListViewSelector (int index);
public interface ListViewDataSource {
    public object Data(int index);
    public int dataCount { get; }
}

public class ListView : MonoBehaviour
{
    [SerializeField]
    private ListViewCell _cellPrefab;
    [SerializeField]
    private ScrollRect _scrollRect;
    [SerializeField]
    private float _cellHeight = -1f;

    private LinkedList<ListViewCell> _cellList = new LinkedList<ListViewCell>();

    private float _lastContentPositionY = 0f;
    private int _index;

    public ListViewSelector selector;
    public ListViewDataSource dataSource;

    void Start()
    {
        // 縦スクロール固定
        _scrollRect.horizontal = false;
        _scrollRect.vertical = true;

        _scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
    }

    void Update()
    {
        if (_cellList.Count == 0)
        {
            return;
        }

        float diff = Mathf.Clamp(_scrollRect.content.anchoredPosition.y - _lastContentPositionY, -_cellHeight, _cellHeight);
        if (Mathf.Abs(diff) >= _cellHeight)
        {
            _lastContentPositionY += diff;

            var firstCell = _cellList.First.Value;
            var lastCell = _cellList.Last.Value;

            if (diff > 0f)
            {
                // 上方向へのスクロール
                if (_index >= dataSource.dataCount - _cellList.Count) {
                    // 最後尾表示してる場合更新しない
                    return;
                }
                if (_lastContentPositionY < _cellHeight) {
                    // 先頭より前にスクロールしてる場合更新しない
                    return;
                }

                _cellList.RemoveFirst();
                _cellList.AddLast(firstCell);
                firstCell.rectTransform.anchoredPosition = new Vector2(0f, lastCell.rectTransform.anchoredPosition.y - _cellHeight);

                var index = _index + _cellList.Count;
                firstCell.UpdateData(index, dataSource.Data(index));

                _index++;
            }
            else
            {
                // 下方向へのスクロール
                if (_index <= 0) {
                    // 先頭表示してる場合更新しない
                    return;
                }
                if (_lastContentPositionY > _cellHeight * (dataSource.dataCount - _cellList.Count + 1)) {
                    // 最後尾より後にスクロールしてる場合更新しない
                    return;
                }

                
                _cellList.RemoveLast();
                _cellList.AddFirst(lastCell);
                lastCell.rectTransform.anchoredPosition = new Vector2(0f, firstCell.rectTransform.anchoredPosition.y + _cellHeight);

                _index--;

                lastCell.UpdateData(_index, dataSource.Data(_index));
            }
        }
    }

    private void SetupCells() {
        var scrollViewHeight = (_scrollRect.transform as RectTransform).rect.height;
        var instantateCellCount = Mathf.CeilToInt(scrollViewHeight / _cellHeight) + 2;
        if (instantateCellCount > dataSource.dataCount) {
            instantateCellCount = dataSource.dataCount;
        }

        for (int i = 0; i < instantateCellCount; i++)
        {
            var cell = Instantiate(_cellPrefab, _scrollRect.content);

            cell.rectTransform.anchorMin = new Vector2(0.5f, 1f);
            cell.rectTransform.anchorMax = new Vector2(0.5f, 1f);
            cell.rectTransform.anchoredPosition = new Vector2(0f, -_cellHeight * i - _cellHeight / 2f);

            cell.UpdateData(i, dataSource.Data(i));
            cell.selector = delegate (int index) {
                selector(index);
            };
            _cellList.AddLast(cell);
        }
        _lastContentPositionY = _scrollRect.content.anchoredPosition.y;
    }

    private void ResetScrollView() {
        _index = 0;
        _scrollRect.verticalNormalizedPosition = 1.0f;
        _lastContentPositionY = 0;
        foreach (var cell in _cellList)
        {
            Destroy(cell);
        }
        _cellList.Clear();
    }

    public void ReloadData() {
        ResetScrollView();
        SetupCells();
    }

    // public void ScrollAtIndex(int index) {
    //     Debug.Log(_cellHeight * index);
    //     if (index <= 0) {
    //         _index = 0;
    //     } else if (index >= dataSource.dataCount - _cellList.Count) {
    //         _index = dataSource.dataCount - _cellList.Count;
    //     }
    //     _scrollRect.verticalNormalizedPosition = _cellHeight * index;
    //     _lastContentPositionY = _cellHeight * index;
    //     Debug.Log("y:" + _cellHeight * index);
    // }
}

public delegate void CellSelector (int index);

public class ListViewCell : MonoBehaviour {

    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private Button _button;

    private int _index;

    public CellSelector selector;
    public RectTransform rectTransform
    {
        get
        {
            return _rectTransform;
        }
    }

    void Start()
    {
        _button.onClick.AddListener(DidSelect);
    }

    public virtual void UpdateData(int index, object obj)
    {
        _index = index;
    }

    private void DidSelect()
    {
         selector(_index);
    }
}