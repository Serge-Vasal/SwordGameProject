using UnityEngine;
using System.Collections;

/*Движение камеры*/
public class MoveMainCamera : MonoBehaviour
{
    public float Speed = 6.0F;
    private CharacterController Controller;
    private Vector3 MoveDirection = Vector3.zero;
    private GameObject MainCamera;
    private float height = 0.3f;
    private GameObject GeneralSettings;

    void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        GeneralSettings = GameObject.FindGameObjectWithTag("GeneralSettings");
        Controller = GetComponent<CharacterController>();
    }

	void Update () 
    {
        LayerMask LayerMap = LayerMask.GetMask("Map");
        Ray ray = new Ray(new Vector3(transform.position.x, 50, transform.position.z), -transform.up); //Пускаем луч для определения высоты земли в данной точке
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 110, LayerMap))
        {
           height = hit.point.y + 0.5f;

        }
        transform.rotation = Quaternion.Euler(0, MainCamera.transform.eulerAngles.y, 0);
        MoveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        MoveDirection = transform.TransformDirection(MoveDirection);
        MoveDirection *= Speed;
        Controller.Move(MoveDirection * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
	}
}
