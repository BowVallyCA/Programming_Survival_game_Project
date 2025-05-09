using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BlockController : MonoBehaviour
{
    //Camera variables

    public Vector2 cameraSpeed = new Vector2(180, 180);
    public float MoveSpeed = 10f;
    private float pitch = 0f;
    private float yaw = 0f;

    //Block variables

    private RaycastHit hit;

    public GameObject blockPrefab;
    public GameObject blockOutLine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Camera stuff

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Temporary camera movement

        pitch += -Input.GetAxis("Mouse Y") * cameraSpeed.x * Time.deltaTime;
        yaw += Input.GetAxis("Mouse X") * cameraSpeed.y * Time.deltaTime;

        Camera.main.transform.eulerAngles = new Vector3(pitch, yaw, 0);

        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Fly");
        float inputZ = Input.GetAxis("Vertical");

        Vector3 dirForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
        Vector3 dirSide = Camera.main.transform.right;
        Vector3 dirUp = Vector3.up;

        Vector3 moveDir = (inputX * dirSide) + (inputY * dirUp) + (inputZ * dirForward);

        Camera.main.transform.position += moveDir * MoveSpeed * Time.deltaTime;

        //Actual block placing

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Physics.Raycast(ray, out hit);

        if(Physics.Raycast(ray,out hit))
        {
            Vector3 pos = hit.point;

            //Grid snapping

            pos += hit.normal * 0.1f;

            pos = new Vector3(
                Mathf.Floor(pos.x),
                Mathf.Floor(pos.y),
                Mathf.Floor(pos.z));

            pos += Vector3.one * 0.5f;

            blockOutLine.transform.position = pos;

            if(Input.GetMouseButtonDown(0))
            {
                Instantiate(blockPrefab, pos, Quaternion.identity);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if(hit.collider.name == "Block(Clone)")
                {
                    Destroy(hit.collider.gameObject);
                }

            }

        }



    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 99999);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hit.point, 0.05f);

        Gizmos.DrawRay(hit.point, hit.normal);
    }
}
