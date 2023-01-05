// Next.js API route support: https://nextjs.org/docs/api-routes/introduction
import type { NextApiRequest, NextApiResponse } from 'next'
import { client } from '../../utils/prisma-client'
import { Client } from '@prisma/client'

export default function handler(
  req: NextApiRequest,
  res: NextApiResponse<Client[]>
) {
  client(async prisma => {
    const clients = await prisma.client.findMany()
    res.status(200).json(clients)
  })
}
