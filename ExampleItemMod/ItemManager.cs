using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExampleItemMod
{
    /// <summary>
    /// Manages item registration, caching, and lifecycle for the Example Item Mod.
    /// Provides centralized control over item creation, retrieval, and management operations.
    /// </summary>
    public class ItemManager : MonoBehaviour
    {
        #region Singleton Pattern
        
        private static ItemManager _instance;
        public static ItemManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ItemManager>();
                    if (_instance == null)
                    {
                        GameObject manager = new GameObject("ItemManager");
                        _instance = manager.AddComponent<ItemManager>();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Fields

        /// <summary>
        /// Dictionary storing registered items by their unique identifier.
        /// </summary>
        private Dictionary<string, Item> _registeredItems = new Dictionary<string, Item>();

        /// <summary>
        /// Dictionary storing item prefabs for instantiation.
        /// </summary>
        private Dictionary<string, GameObject> _itemPrefabs = new Dictionary<string, GameObject>();

        /// <summary>
        /// Lock object for thread-safe operations on item collections.
        /// </summary>
        private readonly object _registrationLock = new object();

        /// <summary>
        /// Indicates whether the ItemManager has been initialized.
        /// </summary>
        private bool _isInitialized = false;

        /// <summary>
        /// Event fired when an item is successfully registered.
        /// </summary>
        public event Action<Item> OnItemRegistered;

        /// <summary>
        /// Event fired when an item is unregistered.
        /// </summary>
        public event Action<string> OnItemUnregistered;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the count of registered items.
        /// </summary>
        public int RegisteredItemCount
        {
            get
            {
                lock (_registrationLock)
                {
                    return _registeredItems.Count;
                }
            }
        }

        /// <summary>
        /// Gets the initialization status.
        /// </summary>
        public bool IsInitialized => _isInitialized;

        #endregion

        #region Lifecycle Methods

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Initialize();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the ItemManager and prepares it for operations.
        /// </summary>
        public void Initialize()
        {
            if (_isInitialized)
            {
                Debug.LogWarning("ItemManager is already initialized.");
                return;
            }

            lock (_registrationLock)
            {
                _registeredItems.Clear();
                _itemPrefabs.Clear();
                _isInitialized = true;

                Debug.Log("[ItemManager] Initialization complete.");
            }
        }

        /// <summary>
        /// Registers a new item with the manager.
        /// </summary>
        /// <param name="item">The item to register.</param>
        /// <returns>True if registration was successful; false if the item ID already exists.</returns>
        public bool RegisterItem(Item item)
        {
            if (item == null)
            {
                Debug.LogError("[ItemManager] Cannot register a null item.");
                return false;
            }

            if (string.IsNullOrEmpty(item.ItemId))
            {
                Debug.LogError("[ItemManager] Item must have a valid ItemId to be registered.");
                return false;
            }

            lock (_registrationLock)
            {
                if (_registeredItems.ContainsKey(item.ItemId))
                {
                    Debug.LogWarning($"[ItemManager] Item with ID '{item.ItemId}' is already registered.");
                    return false;
                }

                _registeredItems[item.ItemId] = item;
                Debug.Log($"[ItemManager] Item '{item.ItemId}' registered successfully.");
                OnItemRegistered?.Invoke(item);
                return true;
            }
        }

        /// <summary>
        /// Registers multiple items at once.
        /// </summary>
        /// <param name="items">Collection of items to register.</param>
        /// <returns>The number of items successfully registered.</returns>
        public int RegisterItems(IEnumerable<Item> items)
        {
            if (items == null)
            {
                Debug.LogError("[ItemManager] Cannot register null item collection.");
                return 0;
            }

            int successCount = 0;
            foreach (var item in items)
            {
                if (RegisterItem(item))
                {
                    successCount++;
                }
            }

            return successCount;
        }

        /// <summary>
        /// Retrieves a registered item by its ID.
        /// </summary>
        /// <param name="itemId">The unique identifier of the item.</param>
        /// <returns>The item if found; null otherwise.</returns>
        public Item GetItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                Debug.LogError("[ItemManager] ItemId cannot be null or empty.");
                return null;
            }

            lock (_registrationLock)
            {
                if (_registeredItems.TryGetValue(itemId, out var item))
                {
                    return item;
                }

                Debug.LogWarning($"[ItemManager] Item with ID '{itemId}' not found.");
                return null;
            }
        }

        /// <summary>
        /// Gets all registered items.
        /// </summary>
        /// <returns>A list of all registered items.</returns>
        public List<Item> GetAllItems()
        {
            lock (_registrationLock)
            {
                return new List<Item>(_registeredItems.Values);
            }
        }

        /// <summary>
        /// Gets items filtered by rarity.
        /// </summary>
        /// <param name="rarity">The rarity level to filter by.</param>
        /// <returns>A list of items matching the specified rarity.</returns>
        public List<Item> GetItemsByRarity(ItemRarity rarity)
        {
            lock (_registrationLock)
            {
                return _registeredItems.Values
                    .Where(item => item.Rarity == rarity)
                    .ToList();
            }
        }

        /// <summary>
        /// Checks if an item with the specified ID is registered.
        /// </summary>
        /// <param name="itemId">The unique identifier to check.</param>
        /// <returns>True if the item is registered; false otherwise.</returns>
        public bool IsItemRegistered(string itemId)
        {
            lock (_registrationLock)
            {
                return _registeredItems.ContainsKey(itemId);
            }
        }

        /// <summary>
        /// Unregisters an item from the manager.
        /// </summary>
        /// <param name="itemId">The unique identifier of the item to unregister.</param>
        /// <returns>True if the item was successfully unregistered; false if not found.</returns>
        public bool UnregisterItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                Debug.LogError("[ItemManager] ItemId cannot be null or empty.");
                return false;
            }

            lock (_registrationLock)
            {
                if (_registeredItems.Remove(itemId))
                {
                    Debug.Log($"[ItemManager] Item '{itemId}' unregistered successfully.");
                    OnItemUnregistered?.Invoke(itemId);
                    return true;
                }

                Debug.LogWarning($"[ItemManager] Item with ID '{itemId}' not found for unregistration.");
                return false;
            }
        }

        /// <summary>
        /// Registers an item prefab for instantiation.
        /// </summary>
        /// <param name="itemId">The unique identifier for the prefab.</param>
        /// <param name="prefab">The prefab GameObject to register.</param>
        /// <returns>True if registration was successful; false otherwise.</returns>
        public bool RegisterItemPrefab(string itemId, GameObject prefab)
        {
            if (string.IsNullOrEmpty(itemId) || prefab == null)
            {
                Debug.LogError("[ItemManager] ItemId and prefab cannot be null or empty.");
                return false;
            }

            lock (_registrationLock)
            {
                if (_itemPrefabs.ContainsKey(itemId))
                {
                    Debug.LogWarning($"[ItemManager] Prefab with ID '{itemId}' is already registered.");
                    return false;
                }

                _itemPrefabs[itemId] = prefab;
                Debug.Log($"[ItemManager] Prefab '{itemId}' registered successfully.");
                return true;
            }
        }

        /// <summary>
        /// Instantiates an item from a registered prefab.
        /// </summary>
        /// <param name="itemId">The unique identifier of the prefab.</param>
        /// <param name="position">The world position for instantiation.</param>
        /// <param name="rotation">The rotation for instantiation.</param>
        /// <returns>The instantiated GameObject, or null if the prefab is not found.</returns>
        public GameObject InstantiateItem(string itemId, Vector3 position, Quaternion rotation)
        {
            lock (_registrationLock)
            {
                if (!_itemPrefabs.TryGetValue(itemId, out var prefab))
                {
                    Debug.LogError($"[ItemManager] Prefab with ID '{itemId}' not found.");
                    return null;
                }

                GameObject instance = Instantiate(prefab, position, rotation);
                Debug.Log($"[ItemManager] Item '{itemId}' instantiated at position {position}.");
                return instance;
            }
        }

        /// <summary>
        /// Clears all registered items and prefabs.
        /// </summary>
        public void ClearAll()
        {
            lock (_registrationLock)
            {
                _registeredItems.Clear();
                _itemPrefabs.Clear();
                Debug.Log("[ItemManager] All items and prefabs cleared.");
            }
        }

        #endregion

        #region Debug Methods

        /// <summary>
        /// Logs all registered items for debugging purposes.
        /// </summary>
        public void LogAllItems()
        {
            lock (_registrationLock)
            {
                Debug.Log($"[ItemManager] Total registered items: {_registeredItems.Count}");
                foreach (var item in _registeredItems.Values)
                {
                    Debug.Log($"  - {item.ItemId}: {item.ItemName}");
                }
            }
        }

        #endregion
    }

    #region Supporting Classes

    /// <summary>
    /// Represents the rarity level of an item.
    /// </summary>
    public enum ItemRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    /// <summary>
    /// Base class for all items in the mod.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Unique identifier for this item.
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// Display name of the item.
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Detailed description of the item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The rarity level of this item.
        /// </summary>
        public ItemRarity Rarity { get; set; }

        /// <summary>
        /// Maximum stack size for this item.
        /// </summary>
        public int MaxStackSize { get; set; } = 1;

        /// <summary>
        /// Weight of a single item unit.
        /// </summary>
        public float Weight { get; set; } = 1f;

        /// <summary>
        /// Path to the item's icon texture resource.
        /// </summary>
        public string IconPath { get; set; }

        /// <summary>
        /// Timestamp when this item was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets a string representation of the item.
        /// </summary>
        public override string ToString()
        {
            return $"Item(ID: {ItemId}, Name: {ItemName}, Rarity: {Rarity})";
        }
    }

    #endregion
}
