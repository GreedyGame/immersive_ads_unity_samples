using UnityEngine;

using UnityEngine.SceneManagement;
public class Runner : MonoBehaviour
{
    private float swipeThreshold = 5f;
    private Vector2 touchStartPos;
    private bool isSwiping = false;
    private float playerSpeed = 8.5f;
    private float leftBoundary = 15.5f ;
    private float rightBoundary = 20.5f;

    private bool endGame = false;

    private void Update()
    {
        if(endGame) return;
        // Player Movement
        transform.Translate(Vector3.forward * playerSpeed * Time.deltaTime);
    #if UNITY_EDITOR
        KeyboardInput();
    #else
        SwipeInput();
    #endif
    }

    private void KeyboardInput()
    {
        // Calculate the next position of the player
        float nextX = Mathf.Clamp(transform.position.x + Input.GetAxisRaw("Horizontal") * playerSpeed * Time.deltaTime, leftBoundary, rightBoundary);

        // Move the player left or right based on the swipe direction within the boundaries
        transform.position = new Vector3(nextX, transform.position.y, transform.position.z);
    }

    private void SwipeInput()
    {
        if (Input.touchCount > 0)
        {
            // Player Movement
            if (isSwiping)
            {
                Vector2 swipeDelta = touchStartPos - (Vector2)Input.touches[0].position;

                if (swipeDelta.magnitude > swipeThreshold) // Adjust the threshold as needed
                {
                    float swipeDirection = Mathf.Sign(swipeDelta.x);
                    // Calculate the next position of the player
                    float nextX = Mathf.Clamp(transform.position.x + -swipeDirection * playerSpeed * Time.deltaTime, leftBoundary, rightBoundary);

                    // Move the player left or right based on the swipe direction within the boundaries
                    transform.position = new Vector3(nextX, transform.position.y, transform.position.z);

                    // Reset the swipe start position
                    touchStartPos = Input.touches[0].position;
                }
            }

            foreach (Touch touch in Input.touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        OnSwipeStart(touch.position);
                        break;
                    case TouchPhase.Ended:
                        OnSwipeEnd();
                        break;
                }
            }
        }
    }

    private void OnSwipeStart(Vector2 touchPos)
    {
        // On swipe start, record the initial position
        touchStartPos = touchPos;
        isSwiping = true;
    }

    private void OnSwipeEnd()
    {
        // On swipe end, reset swipe tracking
        isSwiping = false;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("obstacle"))
        {
            Reset();
            GetComponent<Animator>().SetTrigger("hitObstacle");

        }
        if (other.gameObject.CompareTag("Finish"))
        {
            Reset();
            GetComponent<Animator>().SetTrigger("finish");
        }
    }

    private void Reset()
    {
        endGame = true;
        Invoke("ResetScene", 2f);
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
