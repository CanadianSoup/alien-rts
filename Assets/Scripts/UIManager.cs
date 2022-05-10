using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;

    [SerializeField]
    private Text text = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Display(GameState enteredState)
    {
        var name = enteredState.ToString();
        name = name.Remove(name.IndexOf("State"), 5);

        text.text = name;
    }
}
