using UnityEngine;

public class RadiusKiller : MonoBehaviour, IInteractable
{
    public event System.Action OnCanInteractChanged;
    public bool CanInteract => _isInteractable;

    private bool _isInteractable;

    private RadiusVisual _radiusVisual;   

    private void Awake()
    {
        _radiusVisual = GetComponent<RadiusVisual>();
    } 
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            _isInteractable = true;
            _radiusVisual.VisualizeRadius(true);            
            OnCanInteractChanged?.Invoke();            
        }        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {            
            _isInteractable = false;
            _radiusVisual.VisualizeRadius(false);
            OnCanInteractChanged?.Invoke();            
        }       
    }
}