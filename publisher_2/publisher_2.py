import pika, sys

parameters = pika.URLParameters('amqp://guest:guest@localhost:5672/')
connection = pika.BlockingConnection(parameters)
channel = connection.channel()

channel.exchange_declare(exchange='Deportes', exchange_type='fanout')

message = ' '.join(sys.argv[1:]) or "info: Noticias de Deportes"
channel.basic_publish(exchange='Deportes', routing_key='', body=message)
print(" [x] Sent %r" % message)
connection.close()