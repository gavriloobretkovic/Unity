using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotazione;
    float yRotazione;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //otteniamo la posizione della x del cursore;
        float mouseX = Mouse.current.delta.ReadValue().x * Time.deltaTime * sensX;
        //otteniamo la posizione della y del cursore;
        float mouseY = Mouse.current.delta.ReadValue().y * Time.deltaTime * sensY;

        xRotazione += mouseX;
        yRotazione -= mouseY;
        yRotazione = Mathf.Clamp(yRotazione, -90f, 90f);

        transform.rotation = Quaternion.Euler(yRotazione, xRotazione, 0);
        orientation.rotation = Quaternion.Euler(0, xRotazione, 0);
    }
}
