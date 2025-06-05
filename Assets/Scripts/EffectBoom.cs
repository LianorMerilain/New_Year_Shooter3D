using UnityEngine;
using System.Collections;
public class EffectBoom : MonoBehaviour
{
    [SerializeField] private float _growDuration = 0.3f;
    [SerializeField] private float _destroyDelay = 0.2f;
    [SerializeField] private Vector3 _maxScale = Vector3.one * 2f;
    private float _growTimer;
    private bool _growing;
    private void Awake()
    {
        transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        _growTimer = 0f;
        _growing = true;
        StartCoroutine(DestroyAfterDelay());
    }
    private void Update()
    {
        if (!_growing) return;
        _growTimer += Time.deltaTime;
        float progress = _growTimer / _growDuration;
        transform.localScale = Vector3.Lerp(Vector3.zero, _maxScale, progress);
        if (progress >= 1f)
        {
            _growing = false;
        }
    }
    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(_destroyDelay);
        Destroy(gameObject);
    }
}
