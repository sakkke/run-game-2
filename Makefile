.PHONY: all build clean

all: build

build: clean
	cp -R run-game-2/Build/Build public/unity-build

clean:
	rm -rf public/unity-build
