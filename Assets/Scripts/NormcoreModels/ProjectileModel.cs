using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class ProjectileModel
{
    [RealtimeProperty(1, true, true)] private bool _exploded;
}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class ProjectileModel : RealtimeModel {
    public bool exploded {
        get {
            return _cache.LookForValueInCache(_exploded, entry => entry.explodedSet, entry => entry.exploded);
        }
        set {
            if (this.exploded == value) return;
            _cache.UpdateLocalCache(entry => { entry.explodedSet = true; entry.exploded = value; return entry; });
            InvalidateReliableLength();
            FireExplodedDidChange(value);
        }
    }
    
    public delegate void PropertyChangedHandler<in T>(ProjectileModel model, T value);
    public event PropertyChangedHandler<bool> explodedDidChange;
    
    private struct LocalCacheEntry {
        public bool explodedSet;
        public bool exploded;
    }
    
    private LocalChangeCache<LocalCacheEntry> _cache = new LocalChangeCache<LocalCacheEntry>();
    
    public enum PropertyID : uint {
        Exploded = 1,
    }
    
    public ProjectileModel() : this(null) {
    }
    
    public ProjectileModel(RealtimeModel parent) : base(null, parent) {
    }
    
    protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
        UnsubscribeClearCacheCallback();
    }
    
    private void FireExplodedDidChange(bool value) {
        try {
            explodedDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    protected override int WriteLength(StreamContext context) {
        int length = 0;
        if (context.fullModel) {
            FlattenCache();
            length += WriteStream.WriteVarint32Length((uint)PropertyID.Exploded, _exploded ? 1u : 0u);
        } else if (context.reliableChannel) {
            LocalCacheEntry entry = _cache.localCache;
            if (entry.explodedSet) {
                length += WriteStream.WriteVarint32Length((uint)PropertyID.Exploded, entry.exploded ? 1u : 0u);
            }
        }
        return length;
    }
    
    protected override void Write(WriteStream stream, StreamContext context) {
        var didWriteProperties = false;
        
        if (context.fullModel) {
            stream.WriteVarint32((uint)PropertyID.Exploded, _exploded ? 1u : 0u);
        } else if (context.reliableChannel) {
            LocalCacheEntry entry = _cache.localCache;
            if (entry.explodedSet) {
                _cache.PushLocalCacheToInflight(context.updateID);
                ClearCacheOnStreamCallback(context);
            }
            if (entry.explodedSet) {
                stream.WriteVarint32((uint)PropertyID.Exploded, entry.exploded ? 1u : 0u);
                didWriteProperties = true;
            }
            
            if (didWriteProperties) InvalidateReliableLength();
        }
    }
    
    protected override void Read(ReadStream stream, StreamContext context) {
        while (stream.ReadNextPropertyID(out uint propertyID)) {
            switch (propertyID) {
                case (uint)PropertyID.Exploded: {
                    bool previousValue = _exploded;
                    _exploded = (stream.ReadVarint32() != 0);
                    bool explodedExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.explodedSet);
                    if (!explodedExistsInChangeCache && _exploded != previousValue) {
                        FireExplodedDidChange(_exploded);
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
        _exploded = exploded;
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
