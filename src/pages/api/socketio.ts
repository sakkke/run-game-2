// Next.js API route support: https://nextjs.org/docs/api-routes/introduction
import type { NextApiRequest, NextApiResponse } from 'next'
import { Server } from 'socket.io'
import { client } from '../../utils/prisma-client'

export const config = {
  api: {
    bodyParser: false,
  },
}

export default function handler(
  req: NextApiRequest,
  res: NextApiResponse
) {
  // @ts-ignore
  if (!res.socket.server.io) {
    // @ts-ignore
    const io = new Server(res.socket.server, { path: '/api/socketio' })
    // @ts-ignore
    res.socket.server.io = io

    io.on('connection', socket => {
      const clientId = socket.id

      client(async prisma => {
        await prisma.client.deleteMany({})

        await prisma.client.create({
          data: {
            clientId,
          }
        })
      })

      io.emit('create client', clientId)

      socket.on('disconnect', () => {
        client(async prisma => {
          await prisma.client.delete({
            where: {
              clientId
            }
          })
        })

        io.emit('remove client', clientId)
      })
    })
  }

  res.end()
}
