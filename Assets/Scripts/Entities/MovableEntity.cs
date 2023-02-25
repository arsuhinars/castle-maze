using System.Collections;
using UnityEngine;

public class MovableEntity : MonoBehaviour
{
    [SerializeField] private Transform m_model;
    [SerializeField] private float m_phase = 0f;
    [SerializeField] private MovableSettings m_settings;

    private bool m_leanState = true;

    private LTDescr m_moveLean;
    private LTDescr m_rotationLean;

    public void PlayForward()
    {
        StartCoroutine(StartLean(true));
    }

    public void PlayBackward()
    {
        StartCoroutine(StartLean(false));
    }

    public void Pause()
    {
        m_moveLean.pause();
        m_rotationLean.pause();
    }

    public void Resume()
    {
        m_moveLean.resume();
        m_rotationLean.resume();
    }

    private void Start()
    {
        if (m_settings.finishedFromStart)
        {
            m_leanState = true;
            m_model.transform.localPosition = m_settings.moveOffset;
            m_model.transform.rotation = Quaternion.AngleAxis(
                m_settings.rotationAngle,
                m_settings.rotationAxis
            );
        }
    }

    private void OnEnable()
    {
        StartCoroutine(OnEnableCoroutine());
    }

    private IEnumerator OnEnableCoroutine()
    {
        if (!Mathf.Approximately(m_phase, 0f))
        {
            yield return new WaitForSeconds(
                m_phase - Time.unscaledTime % m_phase
            );
        }

        if (m_settings.playFromStart)
        {
            OnLeanComplete();
        }
    }

    private IEnumerator StartLean(bool isForward=true)
    {
        if (m_moveLean != null && m_rotationLean != null)
        {
            LeanTween.cancel(m_moveLean.uniqueId);
            LeanTween.cancel(m_rotationLean.uniqueId);
        }

        yield return new WaitForSeconds(
            isForward ? m_settings.startDelay : m_settings.endDelay
        );

        m_moveLean = m_model.LeanMoveLocal(
            isForward ? m_settings.moveOffset : Vector3.zero,
            m_settings.animationTime
        );
        m_moveLean.setEase(m_settings.easeType);
        m_moveLean.setLoopOnce();

        m_model.rotation = 
            isForward ?
            Quaternion.identity :
            Quaternion.AngleAxis(
                m_settings.rotationAngle,
                m_settings.rotationAxis
            );

        m_rotationLean = m_model.LeanRotateAroundLocal(
            m_settings.rotationAxis,
            isForward ? m_settings.rotationAngle : -m_settings.rotationAngle,
            m_settings.animationTime
        );
        m_rotationLean.setEase(m_settings.easeType);
        m_rotationLean.setLoopOnce();

        m_moveLean.setOnComplete(OnLeanComplete);
    }

    private void OnLeanComplete()
    {
        if (m_settings.isLooped && isActiveAndEnabled)
        {
            StartCoroutine(StartLean(
                !m_settings.usePingPong || m_leanState
            ));
            m_leanState = !m_leanState;
        }
    }
}
