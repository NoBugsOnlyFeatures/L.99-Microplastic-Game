using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BubbleManager : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _renderer;
    [SerializeField]
    private float scaleSize = 3.0f; // how many times large the bubble is at its highest

    private Vector3 originalScale;
    private float period = 5.0f; // five seconds for a full cycle of small to large
    private float omega;
    private bool _inRange = false;
    private float _rangeThreshold = 0.1f;
    private Color _initialColor;

    public bool BubbleInRange
    {
        get => _inRange;
        private set => _inRange = value;
    }

    void Awake()
    {
        originalScale = transform.localScale;
        omega = (2 * Mathf.PI) / period;
        _renderer = GetComponent<SpriteRenderer>();
        _initialColor = _renderer.material.color;
    }

    void Update()
    {
        var updatedScaleFactor = Mathf.Abs(((float)Mathf.Sin(omega * Time.time) * scaleSize));
        if (scaleSize - updatedScaleFactor <= _rangeThreshold)
        {
            BubbleInRange = true;
            _renderer.material.color = Color.red;
        }
        else
        {
            BubbleInRange = false;
            _renderer.material.color = _initialColor;
        }

        transform.localScale = new Vector3(originalScale.x + updatedScaleFactor,
            originalScale.y + updatedScaleFactor,
            originalScale.z + updatedScaleFactor);
    }
}
