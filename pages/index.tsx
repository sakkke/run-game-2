import Head from 'next/head'
import { useEffect, useState } from 'react'
import Modal from 'react-modal'
import { Inter } from '@next/font/google'
import { Unity, useUnityContext } from 'react-unity-webgl'

const inter = Inter({ subsets: ['latin'] })

enum Scene {
  MainMenu,
  Game,
  Multi,
}

export default function Home() {
  const [isGameOver, setIsGameOver] = useState(false)
  const [scene, setScene] = useState(Scene.MainMenu)

  const handleGameOver = () => {
    setIsGameOver(true)
  }

  const { unityProvider, loadingProgression, isLoaded, addEventListener, removeEventListener } = useUnityContext({
    loaderUrl: 'unity-build/Build.loader.js',
    dataUrl: 'unity-build/Build.data',
    frameworkUrl: 'unity-build/Build.framework.js',
    codeUrl: 'unity-build/Build.wasm'
  })

  useEffect(() => {
    addEventListener('GameOver', handleGameOver)
    return () => void removeEventListener('GameOver', handleGameOver)
  })

  const loadMainMenu = () => {
    setScene(Scene.MainMenu)
  }

  const loadGame = () => {
    setScene(Scene.Game)
  }

  const loadMulti = () => {
    setScene(Scene.Multi)
  }

  return (
    <>
      <style>
        {`@keyframes scroll-x {
          0% {
            background-position: calc(100vh / 73 * 234) 0;
          }
        }`}
      </style>
      <Head>
        <title>Create Next App</title>
        <meta name="description" content="Generated by create next app" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <link rel="icon" href="/favicon.ico" />
      </Head>
      <main
        className="bg-cover flex flex-col h-screen"
        style={{
          // '%.3f' % 1.618 ** 7
          animationDuration: '29.030s',

          animationName: 'scroll-x',
          animationIterationCount: 'infinite',
          animationTimingFunction: 'linear',
          backgroundImage: 'url(/grass-wide-pixel.png)',
        }}
      >
        <Unity
          unityProvider={unityProvider}
          style={{ visibility: isLoaded ? 'visible' : 'hidden' }}
          className="max-h-screen mt-auto"
        />
      </main>
      {scene === Scene.MainMenu ? <>
        <div className="bg-stone-900 fixed grid h-screen place-items-center top-0 w-screen">
          <div className="flex flex-col gap-8 items-center">
            <h1 className={`${inter.className} font-black text-9xl text-stone-50`}>Run Game</h1>
            <button
              className={`${inter.className} bg-stone-500 font-bold h-12 p-4 rounded-full text-stone-50 w-1/2`}
              onClick={loadGame}
            >Play</button>
            <button
              className={`${inter.className} bg-stone-500 font-bold h-12 p-4 rounded-full text-stone-50 w-1/2`}
              onClick={loadMulti}
            >Multi</button>
          </div>
        </div>
      </> : scene === Scene.Game ? <>
        <Modal
          isOpen={isGameOver}
          className="-translate-y-1/2 absolute bg-stone-900/75 gap-8 grid h-fit left-8 p-8 place-items-center right-8 top-1/2"
        >
          <h2 className={`${inter.className} font-black text-9xl text-stone-50`}>Game Over!</h2>
          <button className={`${inter.className} bg-stone-500 font-bold h-12 p-4 rounded-full text-stone-50`}>Restart</button>
          <button
            className={`${inter.className} bg-stone-500 font-bold h-12 p-4 rounded-full text-stone-50`}
            onClick={loadMainMenu}
          >Main Menu</button>
        </Modal>
      </> : scene === Scene.Multi && <>
          <h2>Multi</h2>
      </>}
    </>
  )
}
