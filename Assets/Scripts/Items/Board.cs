using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private GameObject blackPaintPanel;

    public void PaintBoardToBlack()
    {
        blackPaintPanel.gameObject.SetActive(true);
    }
}
