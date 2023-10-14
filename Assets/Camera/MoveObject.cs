using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float moveSpeed = 2f;

    public float pinchDistanceDelta;
    public float pinchDistance;
    public float minPinchDistance = 2;

    void Start()
    {

#if UNITY_IOS
		moveSpeed = 0.05f;
#endif


#if UNITY_ANDROID
		moveSpeed = 0.3f;
#endif

#if UNITY_WEBGL
        if (Application.isMobilePlatform)
            moveSpeed = 0.06f;
#endif
    }



    void LateUpdate()
    {
#if UNITY_ANDROID || UNITY_IOS

		//	if(Input
			float x = Input.GetAxis ("Mouse X");
			float y = Input.GetAxis ("Mouse Y");

			if (Input.touchCount == 2) {
				Touch touch1 = Input.touches[0];
				Touch touch2 = Input.touches[1];

				if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
				{

//				Touch touchZero = Input.GetTouch(0);
//				Touch touchOne = Input.GetTouch(1);
//
//				// Find the position in the previous frame of each touch.
//				Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
//				Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
//
//				// Find the magnitude of the vector (the distance) between the touches in each frame.
//				float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
//				float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

//				float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;


					pinchDistance = Vector2.Distance(touch1.position, touch2.position);
					float prevDistance = Vector2.Distance(touch1.position - touch1.deltaPosition,touch2.position - touch2.deltaPosition);

					pinchDistanceDelta = pinchDistance - prevDistance;




//					Debug.Log("touch one x value is **** "+touch1.deltaPosition.x);
//
//					Debug.Log("touch two x value is **** "+touch2.deltaPosition.x);
//

			//	if (Mathf.Abs(pinchDistanceDelta) < minPinchDistance)
					if((touch1.deltaPosition.x>0 && touch2.deltaPosition.x>0) || (touch1.deltaPosition.x<0 && touch2.deltaPosition.x<0))
					{
						

	//						Debug.Log("entered to difference");
							x = touch1.deltaPosition.x * Time.deltaTime;
	//						y = touchZero.deltaPosition.y * Time.deltaTime;
	//					

							transform.position = transform.position + (Camera.main.transform.TransformDirection( new Vector3 (-x * moveSpeed, 0, 0)));
							transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y,  0f, 1.8f), transform.position.z);

					}


					if((touch1.deltaPosition.y>0 && touch2.deltaPosition.y>0) || (touch1.deltaPosition.y<0 && touch2.deltaPosition.y<0))
					{


						//						Debug.Log("entered to difference");
						y = touch1.deltaPosition.y * Time.deltaTime;
						//						y = touchZero.deltaPosition.y * Time.deltaTime;
						//					

						transform.position = transform.position + (Camera.main.transform.TransformDirection( new Vector3 (0,-y * moveSpeed, 0)));
						transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y,  0f, 1.8f), transform.position.z);

					}

				//			target.transform.position = new Vector3(Mathf.Clamp(target.transform.position.x, -1.5f, 1.4f), Mathf.Clamp(target.transform.position.y, -1.2f, 0.48f), target.transform.position.z);
				}
			}
#endif

#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR || UNITY_WEBGL

        if (Application.isMobilePlatform)
        {
            //	if(Input
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");

            if (Input.touchCount == 2)
            {
                Touch touch1 = Input.touches[0];
                Touch touch2 = Input.touches[1];

                if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
                {

                    //				Touch touchZero = Input.GetTouch(0);
                    //				Touch touchOne = Input.GetTouch(1);
                    //
                    //				// Find the position in the previous frame of each touch.
                    //				Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    //				Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
                    //
                    //				// Find the magnitude of the vector (the distance) between the touches in each frame.
                    //				float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    //				float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                    //				float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;


                    pinchDistance = Vector2.Distance(touch1.position, touch2.position);
                    float prevDistance = Vector2.Distance(touch1.position - touch1.deltaPosition, touch2.position - touch2.deltaPosition);

                    pinchDistanceDelta = pinchDistance - prevDistance;
                    //	if (Mathf.Abs(pinchDistanceDelta) < minPinchDistance)
                    if ((touch1.deltaPosition.x > 0 && touch2.deltaPosition.x > 0) || (touch1.deltaPosition.x < 0 && touch2.deltaPosition.x < 0))
                    {
                        x = touch1.deltaPosition.x * Time.deltaTime;
                        //						y = touchZero.deltaPosition.y * Time.deltaTime;
                        transform.position = transform.position - (Camera.main.transform.TransformDirection(new Vector3(-x * moveSpeed, 0, 0)));
                        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, 0f, 1.8f), transform.position.z);

                    }


                    if ((touch1.deltaPosition.y > 0 && touch2.deltaPosition.y > 0) || (touch1.deltaPosition.y < 0 && touch2.deltaPosition.y < 0))
                    {
                        y = touch1.deltaPosition.y * Time.deltaTime;
                        //						y = touchZero.deltaPosition.y * Time.deltaTime;
                        transform.position = transform.position - (Camera.main.transform.TransformDirection(new Vector3(0, -y * moveSpeed, 0)));
                        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, 0f, 1.8f), transform.position.z);
                    }

                    //			target.transform.position = new Vector3(Mathf.Clamp(target.transform.position.x, -1.5f, 1.4f), Mathf.Clamp(target.transform.position.y, -1.2f, 0.48f), target.transform.position.z);
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.Space))
            {
                float a = Input.GetAxis("Mouse X") * Time.deltaTime;
                float b = Input.GetAxis("Mouse Y") * Time.deltaTime;

                //		transform.position = transform.position + (Camera.main.transform.right + new Vector3 (x * moveSpeed, 0, 0));

                transform.position = transform.position + (Camera.main.transform.TransformDirection(new Vector3(-a * moveSpeed, -b * moveSpeed, 0)));
                //		transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y,  -10f, 10f), transform.position.z);

            }
        }

#endif
    }

}