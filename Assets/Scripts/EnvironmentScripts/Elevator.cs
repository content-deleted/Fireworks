using UnityEngine;

public class Elevator : MonoBehaviour
{
    public GameObject moveElevator;
    public bool moveDown = false;
    public float y;

    public AudioClip elevatorClip;
    AudioSource audioSource;

    void Start()
    {
        y = moveElevator.transform.position.y;
        audioSource = GetComponent<AudioSource>();
        elevatorClip = Resources.Load("elevatorClip") as AudioClip;
    }
    private void OnTriggerStay()
    {
        moveElevator.transform.position += Vector3.up * Time.deltaTime;
        audioSource.PlayOneShot(elevatorClip, 0.7F);
    }

    void Update()
    {
        if (moveDown)
        {
            moveElevator.transform.position += Vector3.down * Time.deltaTime;
            if (moveElevator.transform.position.y <= y)
            {
                moveDown = false;
            }
        }
    }

    private void OnTriggerExit()
    {
        moveDown = true;
        moveElevator.transform.position += Vector3.down * Time.deltaTime;
    }
}