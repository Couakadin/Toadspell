using UnityEngine;

namespace Objects.Runtime
{
    public class ParticleImpactBehaviour : MonoBehaviour
    {
        public void Awake()
        {
            TryGetComponent(out _particleSystem);
            if (_particleSystem == null) return;

            // S'abonner � l'�v�nement lorsque le syst�me de particules s'arr�te
            var mainModule = _particleSystem.main;
            mainModule.stopAction = ParticleSystemStopAction.Callback;
        }

        public void DetachParticles(GameObject parent)
        {
            if (_particleSystem == null || parent == null) return;

            // Sauvegarder le parent actuel
            _originalParent = parent.transform;
            Debug.Log(_originalParent, this);

            // D�tacher et repositionner
            transform.parent = null;
            transform.position = _originalParent.transform.position;

            // Jouer les particules
            _particleSystem.Play();
        }

        private void OnParticleSystemStopped()
        {
            Debug.Log(1);
            // R�assigner le parent d'origine
            transform.parent = _originalParent;
            transform.localPosition = Vector3.zero;
        }

        private ParticleSystem _particleSystem;
        private Transform _originalParent;
    }
}
