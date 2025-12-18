using System;
using System.Collections.Generic;

namespace ExampleItemMod
{
    /// <summary>
    /// Represents the data structure for a Focus item with healing and focus enhancement capabilities.
    /// This class encapsulates all statistical properties related to focus items in the game.
    /// </summary>
    public class FocusItemData
    {
        /// <summary>
        /// Gets or sets the unique identifier for the focus item.
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets the display name of the focus item.
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the description of the focus item's effects.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the rarity level of the focus item.
        /// </summary>
        public int RarityLevel { get; set; }

        /// <summary>
        /// Gets or sets the amount of health restored when the focus item is used.
        /// </summary>
        public int HealAmount { get; set; }

        /// <summary>
        /// Gets or sets the percentage of maximum health restored.
        /// Value should be between 0 and 100.
        /// </summary>
        public float HealPercentage { get; set; }

        /// <summary>
        /// Gets or sets the duration of the heal effect in seconds.
        /// </summary>
        public float HealDuration { get; set; }

        /// <summary>
        /// Gets or sets the amount by which focus stat is increased.
        /// </summary>
        public int FocusBoost { get; set; }

        /// <summary>
        /// Gets or sets the percentage increase to focus regeneration.
        /// </summary>
        public float FocusRegenerationBonus { get; set; }

        /// <summary>
        /// Gets or sets the duration of the focus enhancement in seconds.
        /// </summary>
        public float FocusDuration { get; set; }

        /// <summary>
        /// Gets or sets the cooldown period before the item can be used again, in seconds.
        /// </summary>
        public float CooldownTime { get; set; }

        /// <summary>
        /// Gets or sets the weight of the item for inventory management.
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        /// Gets or sets the value of the item for trading purposes.
        /// </summary>
        public int GoldValue { get; set; }

        /// <summary>
        /// Gets or sets the list of additional effects applied by the focus item.
        /// </summary>
        public List<string> AdditionalEffects { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is consumable.
        /// </summary>
        public bool IsConsumable { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of items that can be stacked in inventory.
        /// </summary>
        public int MaxStackSize { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusItemData"/> class.
        /// </summary>
        public FocusItemData()
        {
            AdditionalEffects = new List<string>();
            ItemId = string.Empty;
            ItemName = string.Empty;
            Description = string.Empty;
            RarityLevel = 1;
            HealAmount = 0;
            HealPercentage = 0f;
            HealDuration = 0f;
            FocusBoost = 0;
            FocusRegenerationBonus = 0f;
            FocusDuration = 0f;
            CooldownTime = 0f;
            Weight = 0f;
            GoldValue = 0;
            IsConsumable = true;
            MaxStackSize = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusItemData"/> class with specified parameters.
        /// </summary>
        /// <param name="itemId">The unique identifier for the item.</param>
        /// <param name="itemName">The display name of the item.</param>
        /// <param name="healAmount">The base health restoration amount.</param>
        /// <param name="focusBoost">The focus stat increase amount.</param>
        public FocusItemData(string itemId, string itemName, int healAmount, int focusBoost)
            : this()
        {
            ItemId = itemId;
            ItemName = itemName;
            HealAmount = healAmount;
            FocusBoost = focusBoost;
        }

        /// <summary>
        /// Calculates the total healing value considering both flat amount and percentage.
        /// </summary>
        /// <param name="maxHealth">The maximum health value to use for percentage calculation.</param>
        /// <returns>The total healing value.</returns>
        public int CalculateTotalHeal(int maxHealth)
        {
            if (maxHealth <= 0)
                return HealAmount;

            int percentageHeal = (int)(maxHealth * HealPercentage / 100f);
            return HealAmount + percentageHeal;
        }

        /// <summary>
        /// Calculates the effective focus regeneration bonus value.
        /// </summary>
        /// <param name="baseRegeneration">The base focus regeneration rate.</param>
        /// <returns>The effective regeneration rate after applying the bonus.</returns>
        public float CalculateEffectiveRegeneration(float baseRegeneration)
        {
            if (baseRegeneration <= 0f)
                return baseRegeneration;

            return baseRegeneration * (1f + FocusRegenerationBonus / 100f);
        }

        /// <summary>
        /// Adds an additional effect to the focus item.
        /// </summary>
        /// <param name="effect">The effect description to add.</param>
        public void AddEffect(string effect)
        {
            if (!string.IsNullOrEmpty(effect) && !AdditionalEffects.Contains(effect))
            {
                AdditionalEffects.Add(effect);
            }
        }

        /// <summary>
        /// Removes an additional effect from the focus item.
        /// </summary>
        /// <param name="effect">The effect description to remove.</param>
        public void RemoveEffect(string effect)
        {
            if (AdditionalEffects.Contains(effect))
            {
                AdditionalEffects.Remove(effect);
            }
        }

        /// <summary>
        /// Gets a string representation of the focus item data.
        /// </summary>
        /// <returns>A formatted string containing the item's key information.</returns>
        public override string ToString()
        {
            return $"FocusItemData: {ItemName} (ID: {ItemId})" +
                   $"\n  Heal: {HealAmount} HP ({HealPercentage}%)" +
                   $"\n  Focus Boost: +{FocusBoost}" +
                   $"\n  Rarity: {RarityLevel}" +
                   $"\n  Value: {GoldValue} Gold";
        }
    }
}
