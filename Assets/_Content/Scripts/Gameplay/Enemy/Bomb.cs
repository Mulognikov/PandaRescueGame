using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Bomb : Enemy
{
    [SerializeField] private Sprite _explosiveSprite;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _sprite;
    private float _explosionTime = 0.075f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    protected override void StartGame()
    {
        _rigidbody.gravityScale = 1;
    }
    
    protected override void StopGame()
    {
        _rigidbody.simulated = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Rigidbody2D rigidbody))
        {
            _rigidbody.simulated = false;
            rigidbody.AddForce((rigidbody.transform.position - transform.position) * HitForce, ForceMode2D.Impulse);
            StartCoroutine(ExplosionCoroutine());
        }
    }

    private IEnumerator ExplosionCoroutine()
    {
        _sprite.sprite = _explosiveSprite;
        yield return new WaitForSeconds(_explosionTime);
        gameObject.SetActive(false);
    }
}
