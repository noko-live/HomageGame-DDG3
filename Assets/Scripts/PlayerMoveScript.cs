using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMoveScript : MonoBehaviour
{
    public static PlayerMoveScript Instance;

    public Rigidbody myRB;
    public float playerSpeed;
    float basePlayerSpeed = 12f;
    Vector3 moveDirection;

    public InputActionReference move;
    public InputActionReference dash;


    [Header("Dash")]
    public float dashPower;
    public float dashDuration;
    public float dashCooldown = 1f;
    bool isDashOnCD = false; 

    bool isDashing = false;

    private void Awake()
    {
        Instance = this;

        myRB = GetComponent<Rigidbody>();
        playerSpeed = basePlayerSpeed;
    }


    private void Update()
    {
        moveDirection = move.action.ReadValue<Vector3>();   

        if (dash.action.ReadValue<float>() != 0)
        {
            if(!isDashing && !isDashOnCD)
            {
                StartCoroutine(Dash());
            }
        }

    }

    private void FixedUpdate()
    {
        myRB.linearVelocity = new Vector3(moveDirection.x * playerSpeed, moveDirection.y, moveDirection.z * playerSpeed);
    }

    IEnumerator Dash()
    {
        isDashing = true;
        playerSpeed *= dashPower;
        myRB.excludeLayers = LayerMask.GetMask("Bullet");
        yield return new WaitForSeconds(dashDuration);

        playerSpeed = basePlayerSpeed;
        myRB.excludeLayers = LayerMask.GetMask("Nothing");
        isDashing = false;

        isDashOnCD = true;
        yield return new WaitForSeconds(dashCooldown);
        isDashOnCD = false;
        
    }

    public bool isPlayerDashing()
    {
        return isDashing;
    }
    public bool isDashOnCooldown()
    {
        return isDashOnCD;
    }

}
