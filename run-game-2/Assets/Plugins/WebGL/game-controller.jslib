var GameController = {
    GameOver: function() {
        dispatchReactUnityEvent('GameOver')
    }
}

mergeInto(LibraryManager.library, GameController)
