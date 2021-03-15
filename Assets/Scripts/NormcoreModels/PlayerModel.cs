using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class PlayerModel
{
    [RealtimeProperty(1, true, true)] private string _playerName;
    [RealtimeProperty(2, true, true)] private float _health;
    [RealtimeProperty(3, true, true)] private Vector3 _forces;
    [RealtimeProperty(4, true, true)] private int _id;
    [RealtimeProperty(5, true, true)] private bool _isBoosting;
}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class PlayerModel : RealtimeModel {
    public string playerName {
        get {
            return _cache.LookForValueInCache(_playerName, entry => entry.playerNameSet, entry => entry.playerName);
        }
        set {
            if (this.playerName == value) return;
            _cache.UpdateLocalCache(entry => { entry.playerNameSet = true; entry.playerName = value; return entry; });
            InvalidateReliableLength();
            FirePlayerNameDidChange(value);
        }
    }
    
    public float health {
        get {
            return _cache.LookForValueInCache(_health, entry => entry.healthSet, entry => entry.health);
        }
        set {
            if (this.health == value) return;
            _cache.UpdateLocalCache(entry => { entry.healthSet = true; entry.health = value; return entry; });
            InvalidateReliableLength();
            FireHealthDidChange(value);
        }
    }
    
    public UnityEngine.Vector3 forces {
        get {
            return _cache.LookForValueInCache(_forces, entry => entry.forcesSet, entry => entry.forces);
        }
        set {
            if (this.forces == value) return;
            _cache.UpdateLocalCache(entry => { entry.forcesSet = true; entry.forces = value; return entry; });
            InvalidateReliableLength();
            FireForcesDidChange(value);
        }
    }
    
    public int id {
        get {
            return _cache.LookForValueInCache(_id, entry => entry.idSet, entry => entry.id);
        }
        set {
            if (this.id == value) return;
            _cache.UpdateLocalCache(entry => { entry.idSet = true; entry.id = value; return entry; });
            InvalidateReliableLength();
            FireIdDidChange(value);
        }
    }
    
    public bool isBoosting {
        get {
            return _cache.LookForValueInCache(_isBoosting, entry => entry.isBoostingSet, entry => entry.isBoosting);
        }
        set {
            if (this.isBoosting == value) return;
            _cache.UpdateLocalCache(entry => { entry.isBoostingSet = true; entry.isBoosting = value; return entry; });
            InvalidateReliableLength();
            FireIsBoostingDidChange(value);
        }
    }
    
    public delegate void PropertyChangedHandler<in T>(PlayerModel model, T value);
    public event PropertyChangedHandler<string> playerNameDidChange;
    public event PropertyChangedHandler<float> healthDidChange;
    public event PropertyChangedHandler<UnityEngine.Vector3> forcesDidChange;
    public event PropertyChangedHandler<int> idDidChange;
    public event PropertyChangedHandler<bool> isBoostingDidChange;
    
    private struct LocalCacheEntry {
        public bool playerNameSet;
        public string playerName;
        public bool healthSet;
        public float health;
        public bool forcesSet;
        public UnityEngine.Vector3 forces;
        public bool idSet;
        public int id;
        public bool isBoostingSet;
        public bool isBoosting;
    }
    
    private LocalChangeCache<LocalCacheEntry> _cache = new LocalChangeCache<LocalCacheEntry>();
    
    public enum PropertyID : uint {
        PlayerName = 1,
        Health = 2,
        Forces = 3,
        Id = 4,
        IsBoosting = 5,
    }
    
    public PlayerModel() : this(null) {
    }
    
    public PlayerModel(RealtimeModel parent) : base(null, parent) {
    }
    
    protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
        UnsubscribeClearCacheCallback();
    }
    
    private void FirePlayerNameDidChange(string value) {
        try {
            playerNameDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireHealthDidChange(float value) {
        try {
            healthDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireForcesDidChange(UnityEngine.Vector3 value) {
        try {
            forcesDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireIdDidChange(int value) {
        try {
            idDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireIsBoostingDidChange(bool value) {
        try {
            isBoostingDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    protected override int WriteLength(StreamContext context) {
        int length = 0;
        if (context.fullModel) {
            FlattenCache();
            length += WriteStream.WriteStringLength((uint)PropertyID.PlayerName, _playerName);
            length += WriteStream.WriteFloatLength((uint)PropertyID.Health);
            length += WriteStream.WriteBytesLength((uint)PropertyID.Forces, WriteStream.Vector3ToBytesLength());
            length += WriteStream.WriteVarint32Length((uint)PropertyID.Id, (uint)_id);
            length += WriteStream.WriteVarint32Length((uint)PropertyID.IsBoosting, _isBoosting ? 1u : 0u);
        } else if (context.reliableChannel) {
            LocalCacheEntry entry = _cache.localCache;
            if (entry.playerNameSet) {
                length += WriteStream.WriteStringLength((uint)PropertyID.PlayerName, entry.playerName);
            }
            if (entry.healthSet) {
                length += WriteStream.WriteFloatLength((uint)PropertyID.Health);
            }
            if (entry.forcesSet) {
                length += WriteStream.WriteBytesLength((uint)PropertyID.Forces, WriteStream.Vector3ToBytesLength());
            }
            if (entry.idSet) {
                length += WriteStream.WriteVarint32Length((uint)PropertyID.Id, (uint)entry.id);
            }
            if (entry.isBoostingSet) {
                length += WriteStream.WriteVarint32Length((uint)PropertyID.IsBoosting, entry.isBoosting ? 1u : 0u);
            }
        }
        return length;
    }
    
    protected override void Write(WriteStream stream, StreamContext context) {
        var didWriteProperties = false;
        
        if (context.fullModel) {
            stream.WriteString((uint)PropertyID.PlayerName, _playerName);
            stream.WriteFloat((uint)PropertyID.Health, _health);
            stream.WriteBytes((uint)PropertyID.Forces, WriteStream.Vector3ToBytes(_forces));
            stream.WriteVarint32((uint)PropertyID.Id, (uint)_id);
            stream.WriteVarint32((uint)PropertyID.IsBoosting, _isBoosting ? 1u : 0u);
        } else if (context.reliableChannel) {
            LocalCacheEntry entry = _cache.localCache;
            if (entry.playerNameSet || entry.healthSet || entry.forcesSet || entry.idSet || entry.isBoostingSet) {
                _cache.PushLocalCacheToInflight(context.updateID);
                ClearCacheOnStreamCallback(context);
            }
            if (entry.playerNameSet) {
                stream.WriteString((uint)PropertyID.PlayerName, entry.playerName);
                didWriteProperties = true;
            }
            if (entry.healthSet) {
                stream.WriteFloat((uint)PropertyID.Health, entry.health);
                didWriteProperties = true;
            }
            if (entry.forcesSet) {
                stream.WriteBytes((uint)PropertyID.Forces, WriteStream.Vector3ToBytes(entry.forces));
                didWriteProperties = true;
            }
            if (entry.idSet) {
                stream.WriteVarint32((uint)PropertyID.Id, (uint)entry.id);
                didWriteProperties = true;
            }
            if (entry.isBoostingSet) {
                stream.WriteVarint32((uint)PropertyID.IsBoosting, entry.isBoosting ? 1u : 0u);
                didWriteProperties = true;
            }
            
            if (didWriteProperties) InvalidateReliableLength();
        }
    }
    
    protected override void Read(ReadStream stream, StreamContext context) {
        while (stream.ReadNextPropertyID(out uint propertyID)) {
            switch (propertyID) {
                case (uint)PropertyID.PlayerName: {
                    string previousValue = _playerName;
                    _playerName = stream.ReadString();
                    bool playerNameExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.playerNameSet);
                    if (!playerNameExistsInChangeCache && _playerName != previousValue) {
                        FirePlayerNameDidChange(_playerName);
                    }
                    break;
                }
                case (uint)PropertyID.Health: {
                    float previousValue = _health;
                    _health = stream.ReadFloat();
                    bool healthExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.healthSet);
                    if (!healthExistsInChangeCache && _health != previousValue) {
                        FireHealthDidChange(_health);
                    }
                    break;
                }
                case (uint)PropertyID.Forces: {
                    UnityEngine.Vector3 previousValue = _forces;
                    _forces = ReadStream.Vector3FromBytes(stream.ReadBytes());
                    bool forcesExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.forcesSet);
                    if (!forcesExistsInChangeCache && _forces != previousValue) {
                        FireForcesDidChange(_forces);
                    }
                    break;
                }
                case (uint)PropertyID.Id: {
                    int previousValue = _id;
                    _id = (int)stream.ReadVarint32();
                    bool idExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.idSet);
                    if (!idExistsInChangeCache && _id != previousValue) {
                        FireIdDidChange(_id);
                    }
                    break;
                }
                case (uint)PropertyID.IsBoosting: {
                    bool previousValue = _isBoosting;
                    _isBoosting = (stream.ReadVarint32() != 0);
                    bool isBoostingExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.isBoostingSet);
                    if (!isBoostingExistsInChangeCache && _isBoosting != previousValue) {
                        FireIsBoostingDidChange(_isBoosting);
                    }
                    break;
                }
                default: {
                    stream.SkipProperty();
                    break;
                }
            }
        }
    }
    
    #region Cache Operations
    
    private StreamEventDispatcher _streamEventDispatcher;
    
    private void FlattenCache() {
        _playerName = playerName;
        _health = health;
        _forces = forces;
        _id = id;
        _isBoosting = isBoosting;
        _cache.Clear();
    }
    
    private void ClearCache(uint updateID) {
        _cache.RemoveUpdateFromInflight(updateID);
    }
    
    private void ClearCacheOnStreamCallback(StreamContext context) {
        if (_streamEventDispatcher != context.dispatcher) {
            UnsubscribeClearCacheCallback(); // unsub from previous dispatcher
        }
        _streamEventDispatcher = context.dispatcher;
        _streamEventDispatcher.AddStreamCallback(context.updateID, ClearCache);
    }
    
    private void UnsubscribeClearCacheCallback() {
        if (_streamEventDispatcher != null) {
            _streamEventDispatcher.RemoveStreamCallback(ClearCache);
            _streamEventDispatcher = null;
        }
    }
    
    #endregion
}
/* ----- End Normal Autogenerated Code ----- */
