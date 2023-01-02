.PHONY: all build clean dev start tokei

all: build

build: clean
	cp -R run-game-2/Build/Build public/unity-build

clean:
	rm -rf public/unity-build

dev:
	watchexec -r -s SIGINT -w run-game-2/Build -w src make start

start:
	npm run build
	npm start

tokei:
	tokei run-game-2/Assets/Scripts run-game-2/Assets/Plugins/WebGL src
