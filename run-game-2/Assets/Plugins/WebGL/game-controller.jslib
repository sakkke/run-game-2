var GameController = {
    GameOver: function() {
        dispatchReactUnityEvent('GameOver')
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
    }
}

mergeInto(LibraryManager.library, GameController)
