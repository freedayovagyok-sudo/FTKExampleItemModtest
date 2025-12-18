using System;

namespace ExampleItemMod
{
    /// <summary>
    /// FocusItem class represents a focus-based item in the FTK game.
    /// This item can be used to enhance player concentration and attention.
    /// </summary>
    public class FocusItem
    {
        /// <summary>
        /// Gets or sets the unique identifier for this focus item.
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets the name of the focus item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of what this focus item does.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the focus boost value provided by this item.
        /// Value range: 0-100 representing percentage increase.
        /// </summary>
        public int FocusBoost { get; set; }

        /// <summary>
        /// Gets or sets the duration in seconds that the focus effect lasts.
        /// </summary>
        public int DurationSeconds { get; set; }

        /// <summary>
        /// Gets or sets the rarity level of this focus item.
        /// </summary>
        public string Rarity { get; set; }

        /// <summary>
        /// Gets or sets the experience points awarded when using this item.
        /// </summary>
        public int ExperienceReward { get; set; }

        /// <summary>
        /// Initializes a new instance of the FocusItem class.
        /// </summary>
        public FocusItem()
        {
            ItemId = Guid.NewGuid().ToString();
            FocusBoost = 0;
            DurationSeconds = 0;
            ExperienceReward = 0;
        }

        /// <summary>
        /// Initializes a new instance of the FocusItem class with specified parameters.
        /// </summary>
        public FocusItem(string name, string description, int focusBoost, int duration, string rarity, int xpReward)
        {
            ItemId = Guid.NewGuid().ToString();
            Name = name;
            Description = description;
            FocusBoost = focusBoost;
            DurationSeconds = duration;
            Rarity = rarity;
            ExperienceReward = xpReward;
        }

        /// <summary>
        /// Activates the focus item and applies its effects.
        /// </summary>
        public void Activate()
        {
            Console.WriteLine($"Activating {Name}: +{FocusBoost}% focus for {DurationSeconds} seconds");
        }

        /// <summary>
        /// Returns a string representation of the focus item.
        /// </summary>
        public override string ToString()
        {
            return $"FocusItem: {Name} (ID: {ItemId}) - Rarity: {Rarity}, Focus Boost: {FocusBoost}%, Duration: {DurationSeconds}s";
        }
    }
}
