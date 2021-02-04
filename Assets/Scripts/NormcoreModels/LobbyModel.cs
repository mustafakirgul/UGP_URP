using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class LobbyModel
{
    [RealtimeProperty(1, true, true)] private string _roomName;
    [RealtimeProperty(2, true, true)] private float _maxPlayers;
    [RealtimeProperty(3, true, true)] private bool _isHost;
}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class LobbyModel : RealtimeModel {
    public string roomName {
        get {
            return _cache.LookForValueInCache(_roomName, entry => entry.roomNameSet, entry => entry.roomName);
        }
        set {
            if (this.roomName == value) return;
            _cache.UpdateLocalCache(entry => { entry.roomNameSet = true; entry.roomName = value; return entry; });
            InvalidateReliableLength();
            FireRoomNameDidChange(value);
        }
    }
    
    public float maxPlayers {
        get {
            return _cache.LookForValueInCache(_maxPlayers, entry => entry.maxPlayersSet, entry => entry.maxPlayers);
        }
        set {
            if (this.maxPlayers == value) return;
            _cache.UpdateLocalCache(entry => { entry.maxPlayersSet = true; entry.maxPlayers = value; return entry; });
            InvalidateReliableLength();
            FireMaxPlayersDidChange(value);
        }
    }
    
    public bool isHost {
        get {
            return _cache.LookForValueInCache(_isHost, entry => entry.isHostSet, entry => entry.isHost);
        }
        set {
            if (this.isHost == value) return;
            _cache.UpdateLocalCache(entry => { entry.isHostSet = true; entry.isHost = value; return entry; });
            InvalidateReliableLength();
            FireIsHostDidChange(value);
        }
    }
    
    public delegate void PropertyChangedHandler<in T>(LobbyModel model, T value);
    public event PropertyChangedHandler<string> roomNameDidChange;
    public event PropertyChangedHandler<float> maxPlayersDidChange;
    public event PropertyChangedHandler<bool> isHostDidChange;
    
    private struct LocalCacheEntry {
        public bool roomNameSet;
        public string roomName;
        public bool maxPlayersSet;
        public float maxPlayers;
        public bool isHostSet;
        public bool isHost;
    }
    
    private LocalChangeCache<LocalCacheEntry> _cache = new LocalChangeCache<LocalCacheEntry>();
    
    public enum PropertyID : uint {
        RoomName = 1,
        MaxPlayers = 2,
        IsHost = 3,
    }
    
    public LobbyModel() : this(null) {
    }
    
    public LobbyModel(RealtimeModel parent) : base(null, parent) {
    }
    
    protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
        UnsubscribeClearCacheCallback();
    }
    
    private void FireRoomNameDidChange(string value) {
        try {
            roomNameDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireMaxPlayersDidChange(float value) {
        try {
            maxPlayersDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireIsHostDidChange(bool value) {
        try {
            isHostDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    protected override int WriteLength(StreamContext context) {
        int length = 0;
        if (context.fullModel) {
            FlattenCache();
            length += WriteStream.WriteStringLength((uint)PropertyID.RoomName, _roomName);
            length += WriteStream.WriteFloatLength((uint)PropertyID.MaxPlayers);
            length += WriteStream.WriteVarint32Length((uint)PropertyID.IsHost, _isHost ? 1u : 0u);
        } else if (context.reliableChannel) {
            LocalCacheEntry entry = _cache.localCache;
            if (entry.roomNameSet) {
                length += WriteStream.WriteStringLength((uint)PropertyID.RoomName, entry.roomName);
            }
            if (entry.maxPlayersSet) {
                length += WriteStream.WriteFloatLength((uint)PropertyID.MaxPlayers);
            }
            if (entry.isHostSet) {
                length += WriteStream.WriteVarint32Length((uint)PropertyID.IsHost, entry.isHost ? 1u : 0u);
            }
        }
        return length;
    }
    
    protected override void Write(WriteStream stream, StreamContext context) {
        var didWriteProperties = false;
        
        if (context.fullModel) {
            stream.WriteString((uint)PropertyID.RoomName, _roomName);
            stream.WriteFloat((uint)PropertyID.MaxPlayers, _maxPlayers);
            stream.WriteVarint32((uint)PropertyID.IsHost, _isHost ? 1u : 0u);
        } else if (context.reliableChannel) {
            LocalCacheEntry entry = _cache.localCache;
            if (entry.roomNameSet || entry.maxPlayersSet || entry.isHostSet) {
                _cache.PushLocalCacheToInflight(context.updateID);
                ClearCacheOnStreamCallback(context);
            }
            if (entry.roomNameSet) {
                stream.WriteString((uint)PropertyID.RoomName, entry.roomName);
                didWriteProperties = true;
            }
            if (entry.maxPlayersSet) {
                stream.WriteFloat((uint)PropertyID.MaxPlayers, entry.maxPlayers);
                didWriteProperties = true;
            }
            if (entry.isHostSet) {
                stream.WriteVarint32((uint)PropertyID.IsHost, entry.isHost ? 1u : 0u);
                didWriteProperties = true;
            }
            
            if (didWriteProperties) InvalidateReliableLength();
        }
    }
    
    protected override void Read(ReadStream stream, StreamContext context) {
        while (stream.ReadNextPropertyID(out uint propertyID)) {
            switch (propertyID) {
                case (uint)PropertyID.RoomName: {
                    string previousValue = _roomName;
                    _roomName = stream.ReadString();
                    bool roomNameExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.roomNameSet);
                    if (!roomNameExistsInChangeCache && _roomName != previousValue) {
                        FireRoomNameDidChange(_roomName);
                    }
                    break;
                }
                case (uint)PropertyID.MaxPlayers: {
                    float previousValue = _maxPlayers;
                    _maxPlayers = stream.ReadFloat();
                    bool maxPlayersExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.maxPlayersSet);
                    if (!maxPlayersExistsInChangeCache && _maxPlayers != previousValue) {
                        FireMaxPlayersDidChange(_maxPlayers);
                    }
                    break;
                }
                case (uint)PropertyID.IsHost: {
                    bool previousValue = _isHost;
                    _isHost = (stream.ReadVarint32() != 0);
                    bool isHostExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.isHostSet);
                    if (!isHostExistsInChangeCache && _isHost != previousValue) {
                        FireIsHostDidChange(_isHost);
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
        _roomName = roomName;
        _maxPlayers = maxPlayers;
        _isHost = isHost;
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
