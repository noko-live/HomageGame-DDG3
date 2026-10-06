using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    GameManagerScript gm;
    Collider myCol;
    Rigidbody myRB;



    private void Awake()
    {
        myCol = GetComponent<Collider>();
        myRB = GetComponent<Rigidbody>();
    }


    private void Start()
    {
        gm = GameManagerScript.Instance;

        myRB.constraints = RigidbodyConstraints.FreezePositionY;
    }

    IEnumerator DeathTImer()
    {
        yield return new WaitForSeconds(8f);

        Destroy(gameObject);

    }





    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.CompareTag("Bullet"))
        {
            Physics.IgnoreCollision(myCol, collision.collider, true);
        }



        if (collision.transform.gameObject.CompareTag("Player"))
        {
            if (!collision.transform.gameObject.GetComponent<PlayerMoveScript>().isPlayerDashing())
            {
                gm.BulletHit();

                Destroy(gameObject);
                Debug.Log("Player Hit");
            }
        }


        if (collision.transform.gameObject.CompareTag("DeathTrigger"))
        {
            Destroy(gameObject);
        }
    }



}
