.PHONY: all build clean start tokei

all: build

build: clean
	cp -R run-game-2/Build/Build public/unity-build

clean:
	rm -rf public/unity-build

start:
	npm run build
	npm start

tokei:
	tokei pages run-game-2/Assets/Scripts run-game-2/Assets/Plugins/WebGL
