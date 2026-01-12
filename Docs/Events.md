# Chip Events

There are two events that are emitted when any chip is equipped or unequipped by a player.

## Events

### OnChipEquipped
This event is fired when a player equips any custom chip.

```csharp
private void OnChipEquipped(TechType techtype, ChipBase chip)
{
    // Your code here
}

// Subscribe to the event
ChipEvents.OnChipEquipped += OnChipEquipped;
```

### OnChipUnequipped
This event is fired when a player unequips any custom chip.
```csharp
private void OnChipUnequipped(TechType techtype, ChipBase chip)
{
    // Your code here
}
// Subscribe to the event
ChipEvents.OnChipUnequipped += OnChipUnequipped;
```
