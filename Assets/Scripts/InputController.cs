using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    KnifeController knifeController = null;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            knifeController.LaunchCurrentKnife();
        }
        if (Input.touches.Length > 0)
        {
            knifeController.LaunchCurrentKnife();
        }
    }
}
