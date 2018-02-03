using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Range(0.1f, 10f)]
    public float Speed = 5;
    
    private Rigidbody2D _body;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate()
    {
        float dx = Input.GetAxis("Horizontal");
        if (dx > 0)
            _body.velocity = new Vector2(Speed, 0);
        else if (dx < 0)
            _body.velocity = new Vector2(-Speed, 0);
        else
        {
            float speedx = _body.velocity.x;
            _body.velocity = new Vector2(Mathf.Lerp(speedx, 0, 0.5f), _body.velocity.y);
        }
    }
}