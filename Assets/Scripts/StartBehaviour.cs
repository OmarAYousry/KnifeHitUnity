using UnityEngine;

public class StartBehaviour : MonoBehaviour
{
    [SerializeField]
    Canvas startCanvas = null;

    [SerializeField]
    InputController inputController = null;

    void Start()
    {
        Time.timeScale = 0.0f;
        inputController.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touches.Length > 0)
        {
            Time.timeScale = 1.0f;
            inputController.enabled = true;
            startCanvas.enabled = false;
            Destroy(this);
        }
    }
}
