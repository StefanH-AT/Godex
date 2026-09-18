using Godex.Logger;
using Godot;

namespace Godex.Input;

public partial class InputStack : Node
{
    public IInputInterceptor? Interceptor { get; set; }
    private IInputInterceptor? GlobalInputsInterceptor { get; set; }

    private readonly Dictionary<int, Stack<IInputHandler>> _handlers;
    private readonly Dictionary<int, int?> _splitscreenIdToInputIdMapping;

    public InputStack()
    {
        _handlers = new Dictionary<int, Stack<IInputHandler>>();
        _splitscreenIdToInputIdMapping = new Dictionary<int, int?>();
    }

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
    }
    public int MaxSplitscreens;
    public bool HasSplitscreens => MaxSplitscreens > 1;

    public void SwitchTo(int splitscreenId, IInputHandler handler)
    {
        ChangeHandler(splitscreenId, handler, true);
    }

    public void Clear()
    {
        foreach (var handlerStack in _handlers.Values)
        {
            foreach (var stack in handlerStack)
            {
                stack.Activatable.Deactivate();
            }
        }
        _handlers.Clear();
    }

    public void Push(int splitscreenId, IInputHandler handler)
    {
        ChangeHandler(splitscreenId, handler, false);
    }

    private void ChangeHandler(int splitscreenId, IInputHandler handler, bool @override = false)
    {
        if (!HasSplitscreens)
        {
            splitscreenId = 0;
        }

        if (!_handlers.ContainsKey(splitscreenId))
        {
            _handlers.Add(splitscreenId, new Stack<IInputHandler>());
        }

        var currentHandler = GetCurrentHandler(splitscreenId);
        if (currentHandler is not null)
        {
            currentHandler.Activatable.Deactivate();
        }
        
        handler.Activatable.Activate();
        if(@override) {
            _handlers[splitscreenId].Clear();
        }
        _handlers[splitscreenId].Push(handler);
    }
    
    public void Pop(int splitscreenId)
    {
        if (!HasSplitscreens)
        {
            splitscreenId = 0;
        }

        if (_handlers.Count == 0)
        {
            GoLogger.GoLog.Warning("Tried to pop interaction handler but the stack is empty");
            return;
        }

        var currentHandler = GetCurrentHandler(splitscreenId);
        if (currentHandler is not null)
        {
            currentHandler.Activatable.Deactivate();
        }
        _handlers[splitscreenId].Pop();
        
        if (currentHandler is not null)
        {
            currentHandler.Activatable.Activate();
        }
    }

    public override void _UnhandledInput(InputEvent input)
    {
        if (GlobalInputsInterceptor != null && GlobalInputsInterceptor.OnInput(input)) return;

        if (Interceptor is not null)
        {
            if (Interceptor.OnInput(input)) return;
        }

        // GoLogger.GoLog.Debug("Input on device ?", input.Device);

        if (HasSplitscreens)
        {
            var inputId = InputDevices.GetInputIdFromInputEvent(input);
            if (inputId is null)
            {
                GoLogger.GoLog.Warning("Unknown input id for device ?", input.Device);
                return;
            }
            int? splitscreenId = GetSplitscreenId(inputId.Value);
            if (splitscreenId.HasValue)
            {
                var currentHandler = GetCurrentHandler(splitscreenId.Value);
                if (currentHandler is null) return;
                currentHandler.OnInput(input);
            }
            else
            {
                var nextSplitscreenId = GetUnmappedSplitscreenId();
                GoLogger.GoLog.Debug("Mapping input ? to splitscreen ?", inputId.Value, nextSplitscreenId);
                _splitscreenIdToInputIdMapping[nextSplitscreenId] = inputId.Value;
            }
        }
        else
        {
            GetCurrentHandler(0)?.OnInput(input);
        }
    }

    public IInputHandler? GetCurrentHandler(int splitscreenId)
    {
        if (!HasSplitscreens)
        {
            splitscreenId = 0;
        }

        if (!_handlers.ContainsKey(splitscreenId))
        {
            return null;
        }
        return _handlers[splitscreenId].Count == 0 ? null : _handlers[splitscreenId].Peek();
    }

    public int? GetInputId(int splitscreenId)
    {
        return _splitscreenIdToInputIdMapping.GetValueOrDefault(splitscreenId);
    }

    public int? GetSplitscreenId(int inputId)
    {
        if (!HasSplitscreens)
        {
            return 0;
        }

        foreach (var mapping in _splitscreenIdToInputIdMapping)
        {
            if (mapping.Value.HasValue && mapping.Value.Value == inputId)
            {
                return mapping.Key;
            }
        }

        return null;
    }

    public bool DoesSplitscreenIdHaveInputId(int inputId)
    {
        return GetInputId(inputId).HasValue;
    }

    public int RegisterNewSplitscreen()
    {
        var newId = _splitscreenIdToInputIdMapping.Count;
        _splitscreenIdToInputIdMapping.Add(newId, null);
        return newId;
    }

    private int GetUnmappedSplitscreenId()
    {
        foreach (var mapping in _splitscreenIdToInputIdMapping)
        {
            if (!mapping.Value.HasValue)
            {
                return mapping.Key;
            }
        }

        return RegisterNewSplitscreen();
    }

}