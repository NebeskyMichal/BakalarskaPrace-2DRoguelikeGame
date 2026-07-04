using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text mana;
    [SerializeField] private SpriteRenderer imageSource;
    [SerializeField] private SpriteRenderer backgroundSource;
    [SerializeField] private GameObject wrapper;
    [SerializeField] private LayerMask dropLayer;

    public Card Card { get; private set; }

    private Vector3 dragStartPosition;
    private Quaternion dragStartRotation;

    public void Setup(Card card)
    {
        Card = card;
        title.text = card.Title;
        description.text = card.Description;
        mana.text = card.Mana.ToString();
        imageSource.sprite = card.Image;
        backgroundSource.sprite = card.CardBackground;
    }

    private void OnMouseEnter()
    {
        if (RunManagerSystem.Instance.IsDeckViewOpen || RunManagerSystem.Instance.AreSettingsOpen) return;
        if (!Interactions.Instance.PlayerCanHover()) return;
        wrapper.SetActive(false);
        Vector3 pos = new(transform.position.x, -2, -5);
        CardViewHoverSystem.Instance.Show(Card, pos);
    }

    private void OnMouseExit()
    {
        if (RunManagerSystem.Instance.IsDeckViewOpen || RunManagerSystem.Instance.AreSettingsOpen) return;
        if (!Interactions.Instance.PlayerCanHover()) return;
        CardViewHoverSystem.Instance.Hide();
        wrapper.SetActive(true);
    }

    private void OnMouseDown()
    {
        if (RunManagerSystem.Instance.IsDeckViewOpen || RunManagerSystem.Instance.AreSettingsOpen) return;
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (Card.ManualTargetEffect != null)
        {
            ManualTargetSystem.Instance.StartTargeting(transform.position);
        }
        else
        {
            Interactions.Instance.PlayerIsDragging = true;
            wrapper.SetActive(true);
            CardViewHoverSystem.Instance.Hide();
            dragStartPosition = transform.position;
            dragStartRotation = transform.rotation;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
        }
    }

    private void OnMouseDrag()
    {
        if (RunManagerSystem.Instance.IsDeckViewOpen || RunManagerSystem.Instance.AreSettingsOpen) return;
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (Card.ManualTargetEffect != null) return;
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
    }

    private void OnMouseUp()
    {
        if (RunManagerSystem.Instance.IsDeckViewOpen || RunManagerSystem.Instance.AreSettingsOpen) return;
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (Card.ManualTargetEffect != null)
        {
            EnemyView target = ManualTargetSystem.Instance.EndTargeting(MouseUtil.GetMousePositionInWorldSpace(-1));
            if (target != null && ManaSystem.Instance.HasEnoughMana(Card.Mana))
            {
                PlayCardGameAction playCardGameAction = new(Card, target);
                ActionSystem.Instance.Perform(playCardGameAction);
            }
        }
        else
        {
            if (ManaSystem.Instance.HasEnoughMana(Card.Mana) && Physics.Raycast(transform.position, Vector3.forward,
                    out RaycastHit hit, 10f, dropLayer))
            {
                PlayCardGameAction playCardGameAction = new(Card, null);
                ActionSystem.Instance.Perform(playCardGameAction);
            }
            else
            {
                transform.position = dragStartPosition;
                transform.rotation = dragStartRotation;
            }

            Interactions.Instance.PlayerIsDragging = false;
        }
    }
}