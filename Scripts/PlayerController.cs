using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 direction;
    private Animator animator;
    [SerializeField] private float speed;
    [SerializeField] private Transform leftHandGrab;
    [SerializeField] private Transform rightHandGrab;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Movement();
        RotateTorwardsMouse();
    }

    void Movement()
    {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //check if input is cancelled and if it is pressed get the direction and start animations
        if (context.canceled)
        {
            direction = Vector3.zero;
            animator.SetBool("Moving", false);
        }
        else
        {
            direction = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);

            //get direction relative to rigidbody's rotation
            Vector3 rbDirection = transform.InverseTransformDirection(direction);

            animator.SetBool("Moving", true);
            animator.SetFloat("HorizontalInput", rbDirection.x);
            animator.SetFloat("VerticalInput", rbDirection.z);
        }
    }

    void RotateTorwardsMouse()
    {
        //rotate a rigidbody torwards a mouse position using raycast but without rotating on y axis
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;

        if (Physics.Raycast(castPoint, out hit, 1000))
        {
            rb.transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }
    }

    private void OnAnimatorIK()
    {
        //set IK position and rotation for left hand
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.8f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0.8f);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandGrab.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandGrab.rotation);

        //set IK position and rotation for right hand
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.8f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0.8f);
        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandGrab.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandGrab.rotation);
    }
}
