using ProMaxUtils;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

public class _2048 : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 startPos;
    [Range(0,1)]
   public float color;
    private void Awake()
    {
        startPos = transform.localPosition;
       // shoot();
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("box"))
        {
           // Sounds.PlaySoundSource(1);
           // Destroy(gameObject);
        }
    }
    private void Update()
    {
        GetComponent<MeshRenderer>().material.color = Color.HSVToRGB(color, 1, 1);
        color += Time.deltaTime;
        if (color > 1)
            color = 0;
    }
        private void OnEnable()
    {
        shoot();
    }
    void shoot()
    {
        transform.localPosition = startPos;
        GetComponent<Pop_Up>().OnButtonClick();
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 angle = Vector2.zero;
        if (angle.x == 0 && angle.z == 0)
        {
            angle.x = Random.Range(-2f, 2f);
            angle.z = Random.Range(-2f, 4f);
        }
        rb.velocity = new Vector3(1, 8, 15);
      //  rb.velocity = Random.value > .1f ? new Vector3(angle.x / 2, 7, angle.z / 2) : new Vector3(angle.x / 2, Random.Range(8, 15), angle.z / 2);
    }
}
