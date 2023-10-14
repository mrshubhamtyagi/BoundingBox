using UnityEngine;

public class CameraOrbit3DViewer : MonoBehaviour
{
    public OrbitSettings settings;

    private bool bScreenTouched;

    private Touch touch;
    float zoomSpeed = 0.3f;

    private OrbitSettings lastSettings;
    private float fixedYValue2D = 10;

    private Camera cam;

    private void SetFixedYValue2D(float _value)
    {
        fixedYValue2D = _value + 1;
    }

    void Awake()
    {
        cam = GetComponent<Camera>();

        Vector3 angles = transform.eulerAngles;
        settings.x = angles.y;
        settings.y = angles.x;



        VariableSetup();
    }
    public void SetAngles(float xRot, float yRot, float distanceToSet)
    {
        settings.x = xRot;
        settings.y = yRot;
        settings.distance = distanceToSet;
    }

    private void VariableSetup()
    {
        #region Platforms Stuff
#if UNITY_ANDROID
		orbitSettings.xSpeed = 25.0f;
		orbitSettings.ySpeed = 15.0f;
#endif

#if UNITY_IOS
		orbitSettings.xSpeed = 7.0f;
		orbitSettings.ySpeed = 6.0f;
		perspectiveZoomSpeed = 0.1f;
#endif

#if UNITY_STANDALONE_WIN || UNITY_WEBGL
        if (Application.isMobilePlatform)
        {
            settings.xSpeed = 5f;
            settings.ySpeed = 3f;
            settings.perspectiveZoomSpeed = 0.1f;
        }
        //orbitSettings.xSpeed = 120.0f;
        //orbitSettings.ySpeed = 50.0f;
        zoomSpeed = 0.3f;
#endif

#if UNITY_STANDALONE_OSX
		orbitSettings.xSpeed = 100.0f;
		orbitSettings.ySpeed = 40.0f;
		zoomSpeed = 0.2f;
#endif
        #endregion
    }

    public void SetTarget(GameObject target)
    {
        settings.target = target.transform;
    }

    void LateUpdate()
    {

        if (settings.target)
        {
#if UNITY_ANDROID || UNITY_IOS// || UNITY_EDITOR
		
        if (Application.isMobilePlatform)
        {
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                touch = Input.GetTouch(0);

                settings.x += touch.deltaPosition.x * settings.xSpeed * Time.deltaTime;
                settings.y -= touch.deltaPosition.y * settings.ySpeed * Time.deltaTime;
            }
            if (Input.touchCount == 2)
            {
                // Store both touches.
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                if (!(touchZero.deltaPosition.x > 0 && touchOne.deltaPosition.x > 0) || !(touchZero.deltaPosition.x < 0 && touchOne.deltaPosition.x < 0) || !(touchZero.deltaPosition.y > 0 && touchOne.deltaPosition.y > 0) || !(touchZero.deltaPosition.y < 0 && touchOne.deltaPosition.y < 0))
                {
                    settings.distance += deltaMagnitudeDiff * settings.perspectiveZoomSpeed * Time.deltaTime;
                }
                settings.distance = Mathf.Clamp(settings.distance, settings.minDistance, settings.maxDistance);
            }
        }
        

#endif

#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR || UNITY_WEBGL
            if (Application.isMobilePlatform)
            {
                if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    touch = Input.GetTouch(0);

                    settings.x -= touch.deltaPosition.x * settings.xSpeed * Time.deltaTime;
                    settings.y += touch.deltaPosition.y * settings.ySpeed * Time.deltaTime;
                }
                if (Input.touchCount == 2)
                {
                    // Store both touches.
                    Touch touchZero = Input.GetTouch(0);
                    Touch touchOne = Input.GetTouch(1);

                    // Find the position in the previous frame of each touch.
                    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    // Find the magnitude of the vector (the distance) between the touches in each frame.
                    float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                    // Find the difference in the distances between each frame.
                    float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                    if (!(touchZero.deltaPosition.x > 0 && touchOne.deltaPosition.x > 0) ||
                        !(touchZero.deltaPosition.x < 0 && touchOne.deltaPosition.x < 0) ||
                        !(touchZero.deltaPosition.y > 0 && touchOne.deltaPosition.y > 0) ||
                        !(touchZero.deltaPosition.y < 0 && touchOne.deltaPosition.y < 0))
                    {
                        settings.distance -= deltaMagnitudeDiff * settings.perspectiveZoomSpeed * Time.deltaTime;
                    }
                    settings.distance = Mathf.Clamp(settings.distance, settings.minDistance, settings.maxDistance);
                }
            }
            else
            {
                if (Input.GetMouseButton(0) && !Input.GetKey(KeyCode.Space))
                {
                    settings.x += Input.GetAxis("Mouse X") * settings.xSpeed * 0.02f;
                    settings.y -= Input.GetAxis("Mouse Y") * settings.ySpeed * 0.02f;
                }

                settings.distance -= Input.GetAxis("Mouse ScrollWheel") * settings.distance * 0.3f;
                settings.distance = Mathf.Clamp(settings.distance, settings.minDistance, settings.maxDistance);
            }
#endif
        }

        settings.y = ClampAngle(settings.y, settings.yMinLimit, settings.yMaxLimit);

        Quaternion rotation = Quaternion.Euler(settings.y, settings.x, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 10f * Time.deltaTime);

        Vector3 vTemp;
        vTemp = new Vector3(0f, 0f, -settings.distance);
        Vector3 position = transform.rotation * vTemp + settings.target.position;

        transform.position = position;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        return Mathf.Clamp(angle, min, max);
    }

}

[System.Serializable]
public class OrbitSettings
{
    public Transform target;
    public float distance = 1.5f;

    // X-Y
    public float x;
    public float y;

    // Speed
    public float xSpeed;
    public float ySpeed;

    // Y min-max
    public int yMinLimit = 10;
    public int yMaxLimit = 75;

    // X min-max
    public int xMinLimit = -180;
    public int xMaxLimit = 180;

    // Distance
    public float minDistance = .5f;
    public float maxDistance = 15f;

    public float perspectiveZoomSpeed = 0.01f;

    // Transform
    [HideInInspector] public Vector3 position;
    [HideInInspector] public Quaternion rotation;


}
