
using UnityEngine;
using System.Collections;

public class RFX4_PhysXSetImpulse : MonoBehaviour
{

    public float Force = 1;
    public ForceMode ForceMode = ForceMode.Force;

    private Rigidbody rig;
    private Transform t;

    public float fallSpeed;

    // Use this for initialization
    void Start () 
    {
     //   rig = GetComponent<Rigidbody>();
	    //t = transform;
	}

	// Update is called once per frame
	void FixedUpdate () 
    {
        // 현재 위치를 아래로 이동
        transform.Translate(Vector3.up * fallSpeed * Time.deltaTime);
        //if(rig!=null) rig.AddForce(t.forward * Force, ForceMode);
    }

    void OnDisable()
    {
        //if (rig!=null)
        //    rig.velocity = Vector3.zero;
    }

    private void OnEnable()
    {
        Vector3 Pos = this.transform.position;
    }
}
