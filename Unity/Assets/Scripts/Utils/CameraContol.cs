using UnityEngine;
using System.Collections;

public class CameraContol : MonoBehaviour
{

    // The camera to control.
    public Camera _camera;
    // The speed the camera moves at.
    public float speed;
    // The 2DRigidbody of the camera.
    public Rigidbody2D rb;
    // The closest (Smallest orthographic size) one can zoom.
    public float zoomMin;
    // The furthest (Largest orthographic size) one can zoom.
    public float zoomMax;
    // The ammount to multiply the users scrollwheel input by to get a reasonable change in zoom (Orthographic size).Cam
    public float scrollMult;
    // The lower bound of movement in x.
    public float xMoveMin;
    // The upper bound of movement in x.
    public float xMoveMax;
    // The lower bound of movement in y.
    public float yMoveMin;
    // The lower bound of movement in y.
    public float yMoveMax;
    // The speed at which the zooming occurs.
    private float zoomSpeed;

    public MapGenerator planeManager;


    // Use this for initialization
    void Start()
    {
        _camera = Camera.main;
        rb = _camera.GetComponent<Rigidbody2D>();
        xMoveMax = (planeManager.size * 2) - 8;
        yMoveMax = (planeManager.size * 2) - 3.5f;
    }

    /*
	* This method allows the camera to zoom in and out by changing the orthographic size of the camera when the mouse wheel is scrolled.
	*/
    void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (_camera.orthographicSize > zoomMax)
        {
            _camera.orthographicSize = zoomMax;
        }
        else if (_camera.orthographicSize < zoomMin)
        {
            _camera.orthographicSize = zoomMin;
        }
        else
        {
            _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, _camera.orthographicSize + scroll * -scrollMult, ref zoomSpeed, 0.2f);
        }

    }

    /*
	* This method allows the camera to move by changing the position and velocity of a 2D Rigidbody when the arrow keys (or wasd) are pressed.
	*/
    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        rb.velocity = movement * speed;

        rb.position = new Vector3(Mathf.Clamp(rb.position.x, xMoveMin, xMoveMax), Mathf.Clamp(rb.position.y, yMoveMin, yMoveMax), 0.0f);

        rb.rotation = 0.0f;
    }

    /*
	* This method, like Update, is called every frame but is used when dealing with rigid bodies.
	*/
    void FixedUpdate()
    {
        Zoom();
        Move();
    }
}