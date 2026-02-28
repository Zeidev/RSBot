using RSBot.Core.Event;

namespace RSBot.Core.Network.Handler.Agent.Inventory;

internal class InventoryItemUseResponse : IPacketHandler
{
    /// <summary>
    ///     Gets or sets the opcode.
    /// </summary>
    /// <value>
    ///     The opcode.
    /// </value>
    public ushort Opcode => 0xB04C;

    /// <summary>
    ///     Gets or sets the destination.
    /// </summary>
    /// <value>
    ///     The destination.
    /// </value>
    public PacketDestination Destination => PacketDestination.Client;

    /// <summary>
    ///     Handles the packet.
    /// </summary>
    /// <param name="packet">The packet.</param>
    public void Invoke(Packet packet)
    {
        var result = packet.ReadByte();
        
        // Handle failure cases - log error and return
        if (result != 0x01)
        {
            var errorCode = result;
            var failedSlot = packet.ReadByte();
            
            // Log the error for debugging
            Log.Debug($"[Inventory] Item use failed: result={errorCode}, slot={failedSlot}");
            
            // Fire event for failure handling (allows plugins to handle retry/logging)
            EventManager.FireEvent("OnUseItemFailed", failedSlot, errorCode);
            return;
        }

        var sourceSlot = packet.ReadByte();
        var newAmount = packet.ReadUShort();

        Game.Player.Inventory.UpdateItemAmount(sourceSlot, newAmount);

        EventManager.FireEvent("OnUseItem", sourceSlot);
    }
}
