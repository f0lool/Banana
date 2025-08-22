using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class EnemyMovement : MonoBehaviour
{
    public float WalkSpeed;
    public float EdgeSafeDistance;
    public Transform PlayerTransform;
    private int _reachEdge;

    private Transform _transform;
    private Rigidbody2D _rigidbody2D;

    public float GetVelocityX()
    {
        return _rigidbody2D.linearVelocity.normalized.x;
    }

    public void Awake()
    {
        _transform = gameObject.GetComponent<Transform>();
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    public void SetPlayer(Transform player)
    {
        PlayerTransform = player;
    }

    public bool CheckGrounded(Vector2 offset)
    {
        Vector2 origin = _transform.position;
        origin += offset;

        float radius = 0.3f;

        Vector2 direction;
        direction.x = 0;
        direction.y = -1;

        float distance = 1.1f;
        LayerMask layerMask = LayerMask.GetMask("Ground");

        RaycastHit2D hitRec = Physics2D.CircleCast(origin, radius, direction, distance, layerMask);
        
        return hitRec.collider != null;
    }

    public void StopMovement()
    {
        _rigidbody2D.linearVelocity = Vector2.zero;
    }

    public bool ChechWall()
    {
        LayerMask layerMask = LayerMask.GetMask("Ground");
        RaycastHit2D hitWall = Physics2D.Raycast(transform.position, Vector2.right * Mathf.Sign(transform.localScale.x), 3, layerMask);

        return hitWall.collider != null;
    }

    public bool CheckGround()
    {
        LayerMask layerMask = LayerMask.GetMask("Ground");
        Vector2 newVector = transform.position;
        newVector.x += 1 * transform.localScale.normalized.x;
        RaycastHit2D hitWall = Physics2D.Raycast(newVector, Vector2.down, 2.5f, layerMask);

        return hitWall.collider != null;
    }

    public void Walk(float move, float modifierSpeed)
    {
        int direction = move > 0 ? 1 : move < 0 ? -1 : 0;

        float newWalkSpeed = (direction == _reachEdge) ? 0 : direction * WalkSpeed * modifierSpeed;

        if (direction != 0)
        {
            Vector3 newScale = _transform.localScale;
            newScale.x = direction * Mathf.Abs(_transform.localScale.x);
            _transform.localScale = newScale;
        }

        Vector2 newVelocity = _rigidbody2D.linearVelocity;
        newVelocity.x = newWalkSpeed;
        _rigidbody2D.linearVelocity = newVelocity;
    }

    private void OnDrawGizmos()
    {
        RaycastHit2D hitWall = Physics2D.Raycast(transform.position, Vector2.right, 3);

        Gizmos.DrawRay(transform.position, Vector2.right * 3 * transform.localScale.x);

        Vector2 newVector = transform.position;
        newVector.x += 1 * transform.localScale.x;


        Gizmos.DrawRay(newVector, new Vector2(0, -2.5f));

        Gizmos.DrawRay(transform.position, Vector2.right * transform.localScale.normalized.x * 10);
    }
}
