var GameController = {
    GameOver: function() {
        dispatchReactUnityEvent('GameOver')
    },
    IncreaseScore: function(score) {
        dispatchReactUnityEvent('IncreaseScore', score)
    },
    InitializeMultiplayer: function() {
        dispatchReactUnityEvent('InitializeMultiplayer')
    },
    ReceiveLocalSettingsParams: function() {
        var json = localStorage.getItem('settingsParams')
        var bufferSize = lengthBytesUTF8(json) + 1
        var buffer = _malloc(bufferSize)
        stringToUTF8(json, buffer, bufferSize)
        return buffer
    },
    SendSettingsParams: function(json) {
        dispatchReactUnityEvent('SendSettingsParams', UTF8ToString(json))
    },
    StartBackgroundAnimation: function() {
        dispatchReactUnityEvent('StartBackgroundAnimation')
    },
    StopBackgroundAnimation: function() {
        dispatchReactUnityEvent('StopBackgroundAnimation')
    }
}

mergeInto(LibraryManager.library, GameController)
