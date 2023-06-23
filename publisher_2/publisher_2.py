import pika, sys

connection = pika.BlockingConnection(pika.ConnectionParameters(
    host='192.168.20.27',
    credentials=pika.PlainCredentials("daniel", "1234")))
channel = connection.channel()

channel.exchange_declare(exchange='logs', exchange_type='fanout')

message = ' '.join(sys.argv[1:]) or "info: Noticias de Deportes"
channel.basic_publish(exchange='logs', routing_key='', body=message)
print(" [x] Sent %r" % message)
connection.close()