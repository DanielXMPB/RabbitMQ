import pika, sys

connection = pika.BlockingConnection(pika.ConnectionParameters(
    host='192.168.20.27',
    credentials=pika.PlainCredentials("daniel", "1234")))

channel = connection.channel()

channel.exchange_declare(exchange='Economia', exchange_type='fanout')
channel.exchange_declare(exchange='Deportes', exchange_type='fanout')
channel.exchange_declare(exchange='Politica', exchange_type='fanout')

result = channel.queue_declare(queue='', exclusive=True)
queue_name = result.method.queue

channel.queue_bind(exchange='Economia', queue=queue_name)
channel.queue_bind(exchange='Deportes', queue=queue_name)
channel.queue_bind(exchange='Politica', queue=queue_name)

print(' [*] Waiting for logs. To exit press CTRL+C')

def callback(ch, method, properties, body):
    print(" [x] %r" % body)

channel.basic_consume(
    queue=queue_name, on_message_callback=callback, auto_ack=True)

channel.start_consuming()