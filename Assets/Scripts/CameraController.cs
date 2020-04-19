using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject fixture;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (fixture)
        {
            Vector2 pos = Vector2.Lerp(transform.position, fixture.transform.position, 5 * Time.deltaTime);
            transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        }
    }
}
