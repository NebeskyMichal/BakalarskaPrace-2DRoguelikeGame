public class Interactions : Singleton<Interactions>
{
    public bool PlayerIsDragging { get; set; } = false;

    
    private void Start()
    {
        PlayerIsDragging = false;
    }
    
    public bool PlayerCanInteract()
    {
        if (!ActionSystem.Instance.IsPerforming) return true;
        return false;
    }

    public bool PlayerCanHover()
    {
        if (PlayerIsDragging) return false;
        return true;
    }
}