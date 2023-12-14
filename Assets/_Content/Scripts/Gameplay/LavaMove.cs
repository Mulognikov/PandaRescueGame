using UnityEngine;

public class LavaMove : MonoBehaviour
{
    [SerializeField] private Transform[] _lava;
    [SerializeField] private Vector2 _offScreenPos;
    [SerializeField] private float _lavaSpeed;

    private void Update()
    {
        foreach (var t in _lava)
        {
            t.position += Vector3.left * _lavaSpeed;
            if (t.localPosition.x < _offScreenPos.x * -1)
            {
                t.localPosition = _offScreenPos;
            }
        }
    }
}
