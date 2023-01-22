import Head from 'next/head'
import { useCallback, useEffect, useState } from 'react'
import Modal from 'react-modal'
import { Inter } from '@next/font/google'
import { Unity, useUnityContext } from 'react-unity-webgl'
import type { ChangeEvent } from 'react'
import { io } from 'socket.io-client'
import { Client } from '@prisma/client'
import { serverSideTranslations } from 'next-i18next/serverSideTranslations'
import { useTranslation } from 'next-i18next'

export async function getStaticProps({ locale }: { locale: string }) {
  return {
    props: {
      ...(await serverSideTranslations(locale, [
        'common',
      ])),
      // Will be passed to the page component as props
    },
  }
}

const inter = Inter({ subsets: ['latin'] })

enum GameEventType {
  Dive,
  Jump,
  MoveBreak,
  MoveLeft,
  MoveRight,
  Squat,
  StandUp,
}

interface IGameEvent {
  type: GameEventType
  clientId: string
}

const GameEvent = class implements IGameEvent {
  type: GameEventType
  clientId: string

  constructor (type: GameEventType, clientId: string) {
    this.type = type
    this.clientId = clientId
  }
}

enum Scene {
  MainMenu,
  Game,
  Multiplayer,
  Settings,
}

interface ISettingsParams {
  cameraSpeed: number
  coinRotateSpeed: number
  gravityX: number
  gravityY: number
  gravityZ: number
  playerDownSpeed: number
  playerJumpPower: number
  playerSpeed: number
  playerSquattingScale: number
}

class SettingsParams implements ISettingsParams {
  cameraSpeed: number
  coinRotateSpeed: number
  gravityX: number
  gravityY: number
  gravityZ: number
  playerDownSpeed: number
  playerJumpPower: number
  playerSpeed: number
  playerSquattingScale: number

  constructor () {
    this.cameraSpeed = 0
    this.coinRotateSpeed = 0
    this.gravityX = 0
    this.gravityY = 0
    this.gravityZ = 0
    this.playerDownSpeed = 0
    this.playerJumpPower = 0
    this.playerSpeed = 0
    this.playerSquattingScale = 0
  }
}

export default function Home() {
  const [isGameOver, setIsGameOver] = useState(false)
  const [scene, setScene] = useState(Scene.MainMenu)

  const [settingsParams, setSettingsParams] = useState(new SettingsParams())

  const [cameraSpeed, setCameraSpeed] = useState(settingsParams.cameraSpeed)
  const [coinRotateSpeed, setCoinRotateSpeed] = useState(settingsParams.coinRotateSpeed)
  const [gravityX, setGravityX] = useState(settingsParams.gravityX)
  const [gravityY, setGravityY] = useState(settingsParams.gravityY)
  const [gravityZ, setGravityZ] = useState(settingsParams.gravityZ)
  const [playerDownSpeed, setPlayerDownSpeed] = useState(settingsParams.playerDownSpeed)
  const [playerJumpPower, setPlayerJumpPower] = useState(settingsParams.playerJumpPower)
  const [playerSpeed, setPlayerSpeed] = useState(settingsParams.playerSpeed)
  const [playerSquattingScale, setPlayerSquattingScale] = useState(settingsParams.playerSquattingScale)

  const [highScore, setHighScore] = useState(0)
  const [score, setScore] = useState(0)
  const [isEnabledBackgroundAnimation, setIsEnabledBackgroundAnimation] = useState(true)

  const { t } = useTranslation('common')

  const handleGameOver = () => {
    setIsGameOver(true)

    if (score > highScore) {
      setHighScore(h => score)
      localStorage.setItem('highScore', `${score}`)
    }
  }

  const updateSettingsParams = (s: ISettingsParams) => {
    setSettingsParams(oldSettingsParams => ({ ...oldSettingsParams, ...s }))

    setCameraSpeed(s.cameraSpeed)
    setCoinRotateSpeed(s.coinRotateSpeed)
    setGravityX(s.gravityX)
    setGravityY(s.gravityY)
    setGravityZ(s.gravityZ)
    setPlayerDownSpeed(s.playerDownSpeed)
    setPlayerJumpPower(s.playerJumpPower)
    setPlayerSpeed(s.playerSpeed)
    setPlayerSquattingScale(s.playerSquattingScale)
  }

  const applyDefaultSettingsParams = () => {
    const defaultJson = localStorage.getItem('defaultSettingsParams')!
    const defaultSettingsParams: ISettingsParams = JSON.parse(defaultJson)

    updateSettingsParams(defaultSettingsParams)
  }

  const applyLocalSettingsParams = () => {
    const json = JSON.stringify(settingsParams)
    const localJson = localStorage.getItem('settingsParams')!
    const localSettingsParams: ISettingsParams = JSON.parse(localJson)

    updateSettingsParams(localSettingsParams)
  }

  const applySettings = () => {
    const newSettingsParams: ISettingsParams = {
      cameraSpeed,
      coinRotateSpeed,
      gravityX,
      gravityY,
      gravityZ,
      playerDownSpeed,
      playerJumpPower,
      playerSpeed,
      playerSquattingScale,
    }

    const newJson = JSON.stringify(newSettingsParams)
    localStorage.setItem('settingsParams', newJson)
  }

  const handleCreateClient = () => {
    const socket = io('/', { path: '/api/socketio' })

    socket.on('connect', () => {
      (async () => {
        const res = await fetch('/api/clients')
        const clients: Client[] = await res.json()
        const ids = clients.map(client => client.clientId)

        for (const id of ids) {
          sendMessage('Game Controller', 'CreateClient', id)
        }
      })()
    })

    socket.on('create client', (clientId: string) => {
      sendMessage('Game Controller', 'CreateClient', clientId)
    })

    socket.on('game event', (gameEventJson: string) => {
      sendMessage('Game Controller', 'ParseEvent', gameEventJson)
    })

    socket.on('remove client', (clientId: string) => {
      sendMessage('Game Controller', 'RemoveClient', clientId)
    })

    const keydownListener = (ev: KeyboardEvent) => {
      if (ev.key === 'ArrowDown') {
        const gameEvent = new GameEvent(GameEventType.Dive, socket.id)
        const json = JSON.stringify(gameEvent)
        socket.emit('game event', json)
      }

      if (ev.key === 'ArrowDown') {
        const gameEvent = new GameEvent(GameEventType.Squat, socket.id)
        const json = JSON.stringify(gameEvent)
        socket.emit('game event', json)
      } else {
        const gameEvent = new GameEvent(GameEventType.StandUp, socket.id)
        const json = JSON.stringify(gameEvent)
        socket.emit('game event', json)
      }

      if (ev.key === 'ArrowLeft') {
        const gameEvent = new GameEvent(GameEventType.MoveLeft, socket.id)
        const json = JSON.stringify(gameEvent)
        socket.emit('game event', json)
      }

      if (ev.key === 'ArrowRight') {
        const gameEvent = new GameEvent(GameEventType.MoveRight, socket.id)
        const json = JSON.stringify(gameEvent)
        socket.emit('game event', json)
      }
    }

    const keypressListener = (ev: KeyboardEvent) => {
      if (ev.repeat) {
        return
      }

      if (ev.key === 'ArrowUp') {
        const gameEvent = new GameEvent(GameEventType.Jump, socket.id)
        const json = JSON.stringify(gameEvent)
        socket.emit('game event', json)
      }
    }

    document.addEventListener('keydown', keydownListener)
    document.addEventListener('keydown', keypressListener)

    return () => {
      document.removeEventListener('keydown', keydownListener)
      document.removeEventListener('keydown', keypressListener)
    }
  }

  const handleIncreaseScore = useCallback((score: number) => {
    setScore(s => s + score)
  }, [score])

  const handleSettingsParams = useCallback((json: string) => {
    const res: ISettingsParams = JSON.parse(json)

    if (localStorage.getItem('defaultSettingsParams') === null) {
      localStorage.setItem('defaultSettingsParams', json)
      localStorage.setItem('settingsParams', json)
    }

    updateSettingsParams(res)
    applyDefaultSettingsParams()
    applyLocalSettingsParams()
  }, [])

  const handleStartBackgroundAnimation = () => {
    setIsEnabledBackgroundAnimation(true)
  }

  const handleStopBackgroundAnimation = () => {
    setIsEnabledBackgroundAnimation(false)
  }

  const resetHighScore = () => {
    setHighScore(h => 0)
  }

  const { unityProvider, loadingProgression, isLoaded, sendMessage, addEventListener, removeEventListener } = useUnityContext({
    loaderUrl: 'unity-build/Build.loader.js',
    dataUrl: 'unity-build/Build.data',
    frameworkUrl: 'unity-build/Build.framework.js',
    codeUrl: 'unity-build/Build.wasm'
  })

  useEffect(() => {
    addEventListener('GameOver', handleGameOver)
    return () => void removeEventListener('GameOver', handleGameOver)
  })

  useEffect(() => {
    addEventListener('SendSettingsParams', handleSettingsParams)
    return () => void removeEventListener('SendSettingsParams', handleSettingsParams)
  }, [addEventListener, removeEventListener, handleSettingsParams])

  useEffect(() => {
    addEventListener('InitializeMultiplayer', handleCreateClient)
    return () => void removeEventListener('InitializeMultiplayer', handleCreateClient)
  })

  useEffect(() => {
    addEventListener('IncreaseScore', handleIncreaseScore)
    return () => void removeEventListener('IncreaseScore', handleIncreaseScore)
  })

  useEffect(() => {
    setHighScore(h => +(localStorage.getItem('highScore') || h))
  }, [])

  useEffect(() => {
    if (scene !== Scene.Game || isGameOver) {
      return
    }

    const id = setInterval(() => {
      setScore(s => s + 1)
    }, 10)

    return () => void clearInterval(id)
  }, [isGameOver, scene])

  useEffect(() => {
    addEventListener('StartBackgroundAnimation', handleStartBackgroundAnimation)
    return () => void removeEventListener('StartBackgroundAnimation', handleStartBackgroundAnimation)
  })

  useEffect(() => {
    addEventListener('StopBackgroundAnimation', handleStopBackgroundAnimation)
    return () => void removeEventListener('StopBackgroundAnimation', handleStopBackgroundAnimation)
  })

  const loadMainMenu = () => {
    sendMessage('Game Controller', 'LoadEmpty')
    setScene(Scene.MainMenu)
  }

  const loadGame = () => {
    setScore(s => 0)
    sendMessage('Game Controller', 'LoadGame')
    setScene(Scene.Game)
  }

  const loadMultiplayer = () => {
    sendMessage('Game Controller', 'LoadMultiplayer')
    setScene(Scene.Multiplayer)
  }

  const loadSettings = () => {
    setScene(Scene.Settings)
  }

  const restartGame = () => {
    setIsGameOver(false)
    loadGame()
  }

  const exitGame = () => {
    loadMainMenu()
    setIsGameOver(false)
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
        <title>{t('ジャガイモラン')}</title>
        <meta name="description" content="Run Game" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <link rel="icon" href="/favicon.svg" type="image/svg+xml" />
        <link rel="manifest" href="manifest.json" />
        <script
          dangerouslySetInnerHTML={{
            __html: `if ('serviceWorker' in navigator) {
              navigator.serviceWorker.register('/sw.js')
            }`
          }}
        />
      </Head>
      <main
        className="bg-cover flex flex-col h-screen"
        style={{
          // '%.3f' % 1.618 ** 7
          animationDuration: '29.030s',

          animationName: 'scroll-x',
          animationIterationCount: 'infinite',
          animationPlayState: isEnabledBackgroundAnimation ? 'running' : 'paused',
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
            <h1 className={`${inter.className} font-black text-9xl text-stone-50`}>{t('ジャガイモラン')}</h1>
            <button
              className={`${inter.className} bg-stone-500 duration-75 font-bold h-12 hover:bg-stone-600 p-4 rounded-full text-stone-50 w-1/2`}
              onClick={loadGame}
            >{t('Play')}</button>
            <button
              className={`${inter.className} bg-stone-500 duration-75 font-bold h-12 hover:bg-stone-600 p-4 rounded-full text-stone-50 w-1/2`}
              onClick={loadMultiplayer}
            >{t('Multiplayer')}</button>
            <button
              className={`${inter.className} bg-stone-500 duration-75 font-bold h-12 hover:bg-stone-600 p-4 rounded-full text-stone-50 w-1/2`}
              onClick={loadSettings}
            >{t('Settings')}</button>
          </div>
        </div>
      </> : scene === Scene.Game ? <>
        <div className="bg-stone-900/75 fixed flex min-w-[200px] mr-4 mt-4 p-4 right-0 rounded top-0">
          <div className={`${inter.className} font-bold text-stone-50`}>{t('Score')}:</div>
          <div className={`${inter.className} flex-grow text-right text-stone-50`}>{score}</div>
        </div>
        <Modal
          isOpen={isGameOver}
          className="-translate-y-1/2 absolute bg-stone-900/75 grid h-fit left-8 p-8 place-items-center right-8 top-1/2"
        >
          <div className="flex flex-col gap-8 items-center">
            <h2 className={`${inter.className} font-black text-9xl text-stone-50`}>{t('Game Over!')}</h2>
            <p className={`${inter.className} font-bold text-stone-50`}>
              {t('Current Score')}: {score}
            </p>
            <p className={`${inter.className} ${score > highScore ? 'font-bold text-green-500' : 'text-stone-50'}`}>
              {score > highScore ? t('New High Score') : t('High Score')}: {highScore}
            </p>
            <button
              className={`${inter.className} bg-stone-500 duration-75 font-bold h-12 hover:bg-stone-600 p-4 rounded-full text-stone-50 w-1/2`}
              onClick={restartGame}
            >{t('Restart')}</button>
            <button
              className={`${inter.className} bg-stone-500 duration-75 font-bold h-12 hover:bg-stone-600 p-4 rounded-full text-stone-50 w-1/2`}
              onClick={exitGame}
            >{t('Main Menu')}</button>
          </div>
        </Modal>
      </> : scene === Scene.Multiplayer ? <>
        <h2>{t('Multiplayer')}</h2>
      </> : scene === Scene.Settings && <>
        <div className="bg-stone-900 fixed grid h-screen overflow-y-scroll p-8 place-items-center top-0 w-screen">
          <div className="flex flex-col gap-8 items-center">
            <h2 className={`${inter.className} font-black text-2xl text-stone-50`}>{t('Settings')}</h2>
            <label className={`${inter.className} text-stone-50`}>
              {t('Gravity X-axis')}: <input
                className="bg-transparent border-b border-b-blue-500 text-right"
                onChange={(e: ChangeEvent<HTMLInputElement>) => void setGravityX(+e.target.value)}
                type="text"
                value={gravityX}
              />
            </label>
            <label className={`${inter.className} text-stone-50`}>
              {t('Gravity Y-axis')}: <input
                className="bg-transparent border-b border-b-blue-500 text-right"
                onChange={(e: ChangeEvent<HTMLInputElement>) => void setGravityY(+e.target.value)}
                type="text"
                value={gravityY}
              />
            </label>
            <label className={`${inter.className} text-stone-50`}>
              {t('Gravity Z-axis')}: <input
                className="bg-transparent border-b border-b-blue-500 text-right"
                onChange={(e: ChangeEvent<HTMLInputElement>) => void setGravityZ(+e.target.value)}
                type="text"
                value={gravityZ}
              />
            </label>
            <label className={`${inter.className} text-stone-50`}>
              {t('Camera Speed')}: <input
                className="bg-transparent border-b border-b-blue-500 text-right"
                onChange={(e: ChangeEvent<HTMLInputElement>) => void setCameraSpeed(+e.target.value)}
                type="text"
                value={cameraSpeed}
              />
            </label>
            <label className={`${inter.className} text-stone-50`}>
              {t('Coin Rotate Speed')}: <input
                className="bg-transparent border-b border-b-blue-500 text-right"
                onChange={(e: ChangeEvent<HTMLInputElement>) => void setCoinRotateSpeed(+e.target.value)}
                type="text"
                value={coinRotateSpeed}
              />
            </label>
            <label className={`${inter.className} text-stone-50`}>
              {t('Player Down Speed')}: <input
                className="bg-transparent border-b border-b-blue-500 text-right"
                onChange={(e: ChangeEvent<HTMLInputElement>) => void setPlayerDownSpeed(+e.target.value)}
                type="text"
                value={playerDownSpeed}
              />
            </label>
            <label className={`${inter.className} text-stone-50`}>
              {t('Player Jump Power')}: <input
                className="bg-transparent border-b border-b-blue-500 text-right"
                onChange={(e: ChangeEvent<HTMLInputElement>) => void setPlayerJumpPower(+e.target.value)}
                type="text"
                value={playerJumpPower}
              />
            </label>
            <label className={`${inter.className} text-stone-50`}>
              {t('Player Speed')}: <input
                className="bg-transparent border-b border-b-blue-500 text-right"
                onChange={(e: ChangeEvent<HTMLInputElement>) => void setPlayerSpeed(+e.target.value)}
                type="text"
                value={playerSpeed}
              />
            </label>
            <label className={`${inter.className} text-stone-50`}>
              {t('Player Squatting Scale')}: <input
                className="bg-transparent border-b border-b-blue-500 text-right"
                onChange={(e: ChangeEvent<HTMLInputElement>) => void setPlayerSquattingScale(+e.target.value)}
                type="text"
                value={playerSquattingScale}
              />
            </label>
            <div className="flex gap-8 w-full">
              <button
                className={`${inter.className} bg-green-500 duration-75 font-bold hover:bg-green-600 min-h-12 p-4 rounded-full text-green-50 w-full`}
                onClick={applySettings}
              >{t('Save & Apply')}</button>
              <button
                className={`${inter.className} bg-red-500 duration-75 font-bold hover:bg-red-600 min-h-12 p-4 rounded-full text-red-50 w-full`}
                onClick={applyDefaultSettingsParams}
              >{t('Reset to default')}</button>
            </div>
            <button
              className={`${inter.className} bg-red-500 duration-75 font-bold h-12 hover:bg-red-600 p-4 rounded-full text-red-50 w-full`}
              onClick={resetHighScore}
            >{t('Reset high score')}</button>
            <p className={`${inter.className} text-stone-50`}>
              {t('Current high score')}: {highScore}
            </p>
            <button
              className={`${inter.className} bg-stone-500 duration-75 font-bold h-12 hover:bg-stone-600 p-4 rounded-full text-stone-50 w-full`}
              onClick={loadMainMenu}
            >{t('Main Menu')}</button>
          </div>
        </div>
      </>}
    </>
  )
}
