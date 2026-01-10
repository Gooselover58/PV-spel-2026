using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Camera))]
public class CameraScript : MonoBehaviour
{
    private Camera cam;
    private Transform playerTrans;
    private Rigidbody2D playerRb;
    private GameObject upsideDownCamObject;

    [SerializeField] Rect fullRect;
    [SerializeField] Rect zoomedRect;

    [SerializeField] float followSpeed;
    [SerializeField, Range(0f, 1f)] float offsetModifier;

    private Vector3 cameraPointOffset;
    private bool shouldFollowPlayer;
    private bool shouldChangeRect;

    private Rect startRect;
    private Rect destinationRect;
    private float changeRectTime;

    private void Awake()
    {
        Global.gameCamScript = this;

        cam = GetComponent<Camera>();
        upsideDownCamObject = transform.GetChild(0).gameObject;

        shouldFollowPlayer = true;
        SetUpsideDownCamera(false);
        ResetView(true);
    }

    private void Start()
    {
        playerTrans = Global.playerTrans;
        playerRb = Global.playerRb;
    }

    private void Update()
    {
        ChangeRect();
        if (Input.GetKeyDown(KeyCode.P))
        {
            SetUpsideDownCamera(true);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SetUpsideDownCamera(false);
        }
    }

    private void FixedUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (shouldFollowPlayer)
        {
            cameraPointOffset = Vector2.zero;//cameraPointOffset = (Vector3)playerRb.velocity * offsetModifier;
            Vector3 camPos = transform.position;
            Vector3 offsetPlayerPos = playerTrans.position + cameraPointOffset;
            Vector3 toPlayer = offsetPlayerPos - transform.position;
            camPos += toPlayer * Time.deltaTime * followSpeed;
            camPos.z = -10;
            transform.position = camPos;
        }
    }

    private void ChangeRect()
    {
        if (shouldChangeRect)
        {
            changeRectTime += Time.deltaTime;
            Rect newRect = new Rect();
            newRect.x = Mathf.Lerp(startRect.x, destinationRect.x, changeRectTime);
            newRect.y = Mathf.Lerp(startRect.y, destinationRect.y, changeRectTime);
            newRect.height = Mathf.Lerp(startRect.height, destinationRect.height, changeRectTime);
            newRect.width = Mathf.Lerp(startRect.width, destinationRect.width, changeRectTime);
            cam.rect = newRect;
        }
    }

    public void SetFullView(bool state)
    {
        startRect = cam.rect;
        destinationRect = (state) ? fullRect : zoomedRect;
        changeRectTime = 0f;
        shouldChangeRect = true;
    }

    private void ResetView(bool full)
    {
        cam.rect = (full) ? fullRect : zoomedRect;
        shouldChangeRect = false;
    }

    public void SetUpsideDownCamera(bool state)
    {
        upsideDownCamObject.SetActive(state);
    }
}
