
using Quantum;
using UnityEngine;
using UnityEngine.UI;

namespace Complete
{

    public class TankRenderer : QuantumCallbacks
    {
        private EntityView entityView;

        private Renderer[] tankMeshes;
        [SerializeField] private GameObject UI;
        [SerializeField] private Slider HealthSlider;
        [SerializeField] private ParticleSystem tankExplosionParticle;

        private void Awake()
        {
            entityView = GetComponent<EntityView>();

            tankMeshes = transform.Find("Body").GetComponentsInChildren<Renderer>(true);

            QuantumEvent.Subscribe<EventOnTankDeath>(this, OnTankDeath);
            QuantumEvent.Subscribe<EventOnTankRespawn>(this, OnTankRespawn);
        }

        private void OnTankDeath(EventOnTankDeath eventData)
        {
            if (!entityView.EntityRef.Equals(eventData.Tank))
                return;

            foreach (Renderer r in tankMeshes)
                r.enabled = false;
            UI.SetActive(false);
            tankExplosionParticle.Play();
        }

        private void OnTankRespawn(EventOnTankRespawn eventData)
        {
            if (!entityView.EntityRef.Equals(eventData.Tank))
                return;

            foreach (Renderer r in tankMeshes)
                r.enabled = true;
            UI.SetActive(true);

            float maxHealth = eventData.MaxHealth.AsFloat;
            HealthSlider.maxValue = maxHealth;
            HealthSlider.value = maxHealth;
            GetComponent<TankHealth>().SetCurrentHealth(maxHealth);
        }
    }

}
