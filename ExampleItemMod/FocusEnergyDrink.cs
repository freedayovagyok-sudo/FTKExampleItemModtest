using System;
using System.Collections.Generic;

namespace ExampleItemMod
{
    /// <summary>
    /// Represents a Focus Energy Drink consumable item that provides healing and focus boost effects.
    /// This item grants temporary bonuses to both health and mental acuity.
    /// </summary>
    public class FocusEnergyDrink
    {
        #region Properties

        /// <summary>
        /// Gets the unique identifier for the Focus Energy Drink item.
        /// </summary>
        public string ItemId { get; private set; }

        /// <summary>
        /// Gets the display name of the item.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the description of the item.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the rarity level of the item (1-5, where 5 is rarest).
        /// </summary>
        public int Rarity { get; private set; }

        /// <summary>
        /// Gets the weight of the item in kilograms.
        /// </summary>
        public double Weight { get; private set; }

        /// <summary>
        /// Gets the base value of the item in gold.
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Gets the amount of health restored when consumed.
        /// </summary>
        public int HealingAmount { get; private set; }

        /// <summary>
        /// Gets the focus boost percentage (0-100).
        /// </summary>
        public int FocusBoostPercentage { get; private set; }

        /// <summary>
        /// Gets the duration of the focus boost effect in seconds.
        /// </summary>
        public int EffectDurationSeconds { get; private set; }

        /// <summary>
        /// Gets the maximum quantity that can be stacked.
        /// </summary>
        public int MaxStackSize { get; private set; }

        /// <summary>
        /// Gets whether the item is consumable (used up upon consumption).
        /// </summary>
        public bool IsConsumable { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the FocusEnergyDrink class with default values.
        /// </summary>
        public FocusEnergyDrink()
        {
            ItemId = "item_focus_energy_drink";
            Name = "Focus Energy Drink";
            Description = "A refreshing energy drink that restores vitality and sharpens mental clarity. " +
                          "Provides immediate healing and temporary focus enhancement for tactical decision-making.";
            Rarity = 2;
            Weight = 0.5;
            Value = 150;
            HealingAmount = 35;
            FocusBoostPercentage = 25;
            EffectDurationSeconds = 300; // 5 minutes
            MaxStackSize = 20;
            IsConsumable = true;
        }

        /// <summary>
        /// Initializes a new instance of the FocusEnergyDrink class with custom healing parameters.
        /// </summary>
        /// <param name="healingAmount">The amount of health to restore.</param>
        /// <param name="focusBoostPercentage">The focus boost percentage.</param>
        /// <param name="durationSeconds">The duration of effects in seconds.</param>
        public FocusEnergyDrink(int healingAmount, int focusBoostPercentage, int durationSeconds) : this()
        {
            HealingAmount = ValidateHealingAmount(healingAmount);
            FocusBoostPercentage = ValidateFocusBoost(focusBoostPercentage);
            EffectDurationSeconds = ValidateDuration(durationSeconds);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Applies the effects of the Focus Energy Drink to a character.
        /// </summary>
        /// <param name="character">The character to apply effects to.</param>
        /// <returns>True if effects were successfully applied, false otherwise.</returns>
        public bool ConsumeItem(Character character)
        {
            if (character == null)
            {
                throw new ArgumentNullException(nameof(character), "Character cannot be null.");
            }

            try
            {
                // Apply healing effect
                character.Heal(HealingAmount);

                // Apply focus boost effect
                character.ApplyFocusBoost(FocusBoostPercentage, EffectDurationSeconds);

                // Log consumption
                LogConsumption(character.Name);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error consuming Focus Energy Drink: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets a detailed summary of the item's effects.
        /// </summary>
        /// <returns>A formatted string describing the item's effects.</returns>
        public string GetEffectsSummary()
        {
            return $"Healing: +{HealingAmount} HP\n" +
                   $"Focus Boost: +{FocusBoostPercentage}%\n" +
                   $"Duration: {EffectDurationSeconds} seconds\n" +
                   $"Value: {Value} gold";
        }

        /// <summary>
        /// Gets the effective value of the item considering rarity and effects.
        /// </summary>
        /// <returns>The calculated effective value.</returns>
        public int GetEffectiveValue()
        {
            double rarityMultiplier = 1.0 + (Rarity * 0.1);
            double effectValue = HealingAmount * 3 + FocusBoostPercentage * 2;
            return (int)(Value + (effectValue * rarityMultiplier));
        }

        /// <summary>
        /// Gets detailed information about the item.
        /// </summary>
        /// <returns>A formatted string with complete item information.</returns>
        public override string ToString()
        {
            return $"[{ItemId}]\n" +
                   $"Name: {Name}\n" +
                   $"Description: {Description}\n" +
                   $"Rarity: {Rarity}/5\n" +
                   $"Weight: {Weight} kg\n" +
                   $"Base Value: {Value} gold\n" +
                   $"Healing: {HealingAmount} HP\n" +
                   $"Focus Boost: {FocusBoostPercentage}%\n" +
                   $"Effect Duration: {EffectDurationSeconds}s\n" +
                   $"Max Stack: {MaxStackSize}\n" +
                   $"Consumable: {(IsConsumable ? "Yes" : "No")}";
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Validates the healing amount is within acceptable range.
        /// </summary>
        private int ValidateHealingAmount(int amount)
        {
            const int MIN_HEALING = 1;
            const int MAX_HEALING = 200;

            if (amount < MIN_HEALING)
                return MIN_HEALING;
            if (amount > MAX_HEALING)
                return MAX_HEALING;

            return amount;
        }

        /// <summary>
        /// Validates the focus boost percentage is within 0-100 range.
        /// </summary>
        private int ValidateFocusBoost(int percentage)
        {
            if (percentage < 0)
                return 0;
            if (percentage > 100)
                return 100;

            return percentage;
        }

        /// <summary>
        /// Validates the effect duration is reasonable.
        /// </summary>
        private int ValidateDuration(int seconds)
        {
            const int MIN_DURATION = 10;
            const int MAX_DURATION = 3600; // 1 hour

            if (seconds < MIN_DURATION)
                return MIN_DURATION;
            if (seconds > MAX_DURATION)
                return MAX_DURATION;

            return seconds;
        }

        /// <summary>
        /// Logs the consumption of the item.
        /// </summary>
        private void LogConsumption(string characterName)
        {
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($"[{timestamp}] {characterName} consumed {Name}");
        }

        #endregion
    }

    /// <summary>
    /// Simple Character class for demonstration purposes.
    /// </summary>
    public class Character
    {
        public string Name { get; set; }
        public int CurrentHealth { get; set; }
        public int MaxHealth { get; set; }
        public int FocusLevel { get; set; }

        public Character(string name, int maxHealth = 100, int focusLevel = 50)
        {
            Name = name;
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            FocusLevel = focusLevel;
        }

        public void Heal(int amount)
        {
            CurrentHealth = Math.Min(CurrentHealth + amount, MaxHealth);
        }

        public void ApplyFocusBoost(int boostPercentage, int durationSeconds)
        {
            int boostAmount = (int)(FocusLevel * boostPercentage / 100.0);
            FocusLevel = Math.Min(FocusLevel + boostAmount, 100);
            Console.WriteLine($"{Name}'s focus increased by {boostPercentage}% for {durationSeconds} seconds.");
        }
    }
}
