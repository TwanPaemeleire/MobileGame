using UnityEngine;

public class CustomScrollView : MonoBehaviour
{
    [SerializeField] private float _topMaxExcessScroll;
    [SerializeField] private float _bottomMaxExcessScroll;
    private void Update()
    {
        if(true) // Any input in window
        {
            // Follow touch with loaderboard with no delay

            // Unless we're in the excess area, then we slow down more and more the closer we are to the max
        }
        else if(true) // Processing a swipe
        {
            // Go into swipe direction and gradually slow down
        }
        else if (true) // No input
        {
            if(true) // Check if we're in the excess areas
            {
                // Send player back to non-excess area
            }
        }
    }
}
