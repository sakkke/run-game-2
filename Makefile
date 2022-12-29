.PHONY: all build clean dev start tokei

all: build

build: clean
	cp -R run-game-2/Build/Build public/unity-build

clean:
	rm -rf public/unity-build

dev:
	watchexec -r -s SIGINT -w pages -w run-game-2/Build -w styles make start

start:
	npm run build
	npm start

tokei:
	tokei pages run-game-2/Assets/Scripts run-game-2/Assets/Plugins/WebGL styles
