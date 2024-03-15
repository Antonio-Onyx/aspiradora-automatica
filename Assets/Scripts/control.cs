using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class control : MonoBehaviour
{
    public float speed;
    public int contador;
    Rigidbody rb;
    public Text count;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count.text = contador.ToString();
    }

    void FixedUpdate(){
        float MoveHorizontal = Input.GetAxis("Horizontal");
        float MoveVertical = Input.GetAxis("Vertical");

        // vector3 = (x,y,z)
        Vector3 movimiento = new Vector3(MoveHorizontal,0.0f,MoveVertical);
        rb.AddForce(movimiento*speed);
        count.text = contador.ToString();
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("point")){
            Destroy(other.gameObject);
            contador++;
        }
    }
}
