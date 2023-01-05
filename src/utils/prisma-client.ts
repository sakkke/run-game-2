import { PrismaClient } from '@prisma/client'

const prisma = new PrismaClient()

export async function client(callback: (prisma: PrismaClient) => Promise<void>) {
  return callback(prisma)
    .then(async () => {
        await prisma.$disconnect()
    })
    .catch(async (e) => {
        console.error(e)
        await prisma.$disconnect()
        process.exit(1)
    })
}
