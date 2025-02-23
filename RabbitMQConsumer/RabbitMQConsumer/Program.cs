
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

//Bağlantı oluşturma

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rvvkxluh:xZyCL6Ea-2UgGTTGgfLnOSRyXyiZnXN3@hawk.rmq.cloudamqp.com/rvvkxluh");

//Bağlantı AKtifleştirme ve Kanal AÇma
using IConnection connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();
//Queue oluşturma
await channel.QueueDeclareAsync(queue:"umut",exclusive:false,autoDelete: false,durable:true); //Consumerda' da kuyruk publishe ile birebir aynı yapıda tanımlanmalıdır

//Queue'den Mesaj Okuma
AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);
await channel.BasicConsumeAsync(queue:"umut", autoAck: false, consumer: consumer); //auto ack parametresi ile mesajın kuyruktan silinmesi için consumer'dan onay bekleyecektir
//Fair Dispatch davranışı konfigürasyonu
await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);
//prefetchSize : consumer tarafından alınabilecek en büyük mesaj boyutunu byte cinsinden belirler. 0 sınırsız demek
//prefetchCount: Consumer tarafından aynı anda işleme alınabilecek mesaj sayısı
//global: Tüm consumerlar için mi yoksa çağrı yapılan ilgili consumer için mi geçerli olduğunu belirler
consumer.ReceivedAsync += async (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
    
    await channel.BasicAckAsync(e.DeliveryTag, false); // işlenen bu mesajın silineceğini bildiririz 
    //multipile a false dediğimizde sadece bu mesaja dair bir çalışma yapacağımı bildiririz
    
};

Console.ReadLine();