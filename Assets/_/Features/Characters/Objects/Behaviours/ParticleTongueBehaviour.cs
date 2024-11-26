using UnityEngine;
using Data.Runtime;

public class ParticleTongueBehaviour : MonoBehaviour, IParticle
{
    public void PlayParticle()
    {
        if (_type == ParticleType.medium)
        {
            _particle1M?.Play();
            _particle2M?.Play();
        }
        else if (_type == ParticleType.large)
        {
            _particle1L.Play();
            _particle2L.Play();
        }
        else return;
    }

    public void StopParticle()
    {
        if (_type == ParticleType.medium)
        {
            _particle1M.Stop();
            _particle2M.Stop();
        }
        else if (_type == ParticleType.large)
        {
            _particle1L.Stop();
            _particle2L.Stop();
        }
        else return;
    }

    private enum ParticleType {
        none,
        medium,
        large
    }

    [SerializeField]
    private ParticleType _type;

    [SerializeField]
    private ParticleSystem _particle1M, _particle2M, _particle1L, _particle2L;
}
