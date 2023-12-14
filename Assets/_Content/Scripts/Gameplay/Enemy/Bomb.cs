using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ParticleSystem))]
public class Bomb : Enemy, IBeeKiller
{
    private SoundController _soundController;
    
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private ParticleSystem _particleSystem;

    [Inject]
    private void Construct(SoundController soundController)
    {
        _soundController = soundController;
    }
    
    protected override void Awake()
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

    protected override void SurvivorHit(Survivor survivor, Vector3 hitPosition)
    {
        return;
    }
    
    protected override void RigidbodyHit(Rigidbody2D rigidbody)
    {
        base.RigidbodyHit(rigidbody);
        _rigidbody.simulated = false;
        StartCoroutine(ExplosionCoroutine());
    }

    private IEnumerator ExplosionCoroutine()
    {
        _soundController.BombSound();
        _particleSystem.Play();
        _animator.enabled = true;
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length);
        gameObject.SetActive(false);
    }
}
