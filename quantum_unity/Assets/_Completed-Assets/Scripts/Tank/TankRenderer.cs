
using Quantum;
using UnityEngine;

public class TankRenderer : QuantumCallbacks
{
    public EntityView entityView;

    public Renderer[] tankMeshes;
    public Renderer[] tankUi;

    private void Awake()
    {
        entityView = GetComponent<EntityView>();

        tankMeshes = transform.Find("Body").GetComponentsInChildren<Renderer>(true);
        tankUi = transform.Find("UI").GetComponentsInChildren<Renderer>();

        QuantumEvent.Subscribe<EventOnTankDeath>(this, OnTankDeath);
        QuantumEvent.Subscribe<EventOnTankRespawn>(this, OnTankRespawn);
    }

    private void OnTankDeath(EventOnTankDeath eventData)
    {
        if (!entityView.EntityRef.Equals(eventData.Tank))
            return;

        foreach (Renderer r in tankMeshes)
            r.enabled = false;
        foreach (Renderer r in tankUi)
            r.enabled = false;
    }

    private void OnTankRespawn(EventOnTankRespawn eventData)
    {
        if (!entityView.EntityRef.Equals(eventData.Tank))
            return;

        foreach (Renderer r in tankMeshes)
            r.enabled = true;
        foreach (Renderer r in tankUi)
            r.enabled = true;
    }
}
