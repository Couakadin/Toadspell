using UnityEngine;

namespace Objects.Runtime
{
    public class ParticleImpactBehaviour : MonoBehaviour
    {
        public void Awake()
        {
            TryGetComponent(out _particleSystem);
            if (_particleSystem == null) return;

            // S'abonner à l'événement lorsque le système de particules s'arrête
            var mainModule = _particleSystem.main;
            mainModule.stopAction = ParticleSystemStopAction.Callback;
        }

        public void DetachParticles(GameObject parent)
        {
            if (_particleSystem == null || parent == null) return;

            // Sauvegarder le parent actuel
            _originalParent = parent.transform;

            // Détacher et repositionner
            transform.parent = null;
            transform.position = _originalParent.transform.position;

            // Jouer les particules
            _particleSystem.Play();
        }

        private void OnParticleSystemStopped()
        {
            // Réassigner le parent d'origine
            transform.parent = _originalParent;
            transform.localPosition = Vector3.zero;
        }

        private ParticleSystem _particleSystem;
        private Transform _originalParent;
    }
}
