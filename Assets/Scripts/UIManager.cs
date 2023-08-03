using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private Image circleImage;

    private void Awake()
    {
        if(Instance != null)
        {
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void StartDoorCircleAnimation()
    {
        circleImage.gameObject.SetActive(true);

        LeanTween.value(circleImage.gameObject, 0f, 1f, 0.5f)
        .setLoopPingPong(-1)
        .setOnUpdate((float val) => {
            var c = circleImage.color;
            c.a = val;
            circleImage.color = c;
        });
    }

    public void OnLevelCompleted()
    {
        LeanTween.cancel(circleImage.gameObject);
        circleImage.gameObject.SetActive(false);

        Debug.Log("level completed");
    }
}
