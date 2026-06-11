using UnityEngine;

public class PickupInteractionBehavior : IInteractableBehavior
{
    public void PerformInteraction(InteractionData data)
    {
        if (true /* !player.HasObject */)
        {
            // Agarrar objeto
            Debug.Log("Objeto recogido");
        }
        else
        {
            // Intentar soltar
            if (IsValidDropTile(data.TargetTile))
            {
                Debug.Log("Objeto soltado");
            }
            else
            {
                Debug.Log("No se puede soltar aquí");
            }
        }
    }

    private bool IsValidDropTile(Vector2Int tile)
    {
        // Lógica para verificar si el tile es válido para soltar el objeto
        return true;
    }
}
