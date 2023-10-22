using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ParticleSystem))]
public class Bomb : Enemy
{
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private ParticleSystem _particleSystem;
    private float _explosionTime = 0.075f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _particleSystem = GetComponent<ParticleSystem>();
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
        _particleSystem.Play();
        _animator.enabled = true;
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length);
        gameObject.SetActive(false);
    }
}
